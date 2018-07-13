
CREATE PROCEDURE [dbo].[ImportFond] @codeFond nvarchar(200), @path nvarchar(max)
AS 
BEGIN
DECLARE @FondId INT

DEClARE @tableTemp nvarchar(200) = 'Fond_' + @codeFond
DECLARE @scriptCreate NVARCHAR(MAX)
DECLARE @scriptBulk NVARCHAR(MAX)
DECLARE @script NVARCHAR(MAX)


SELECT @FondId = Id FROM Fond WHERE Code = @codeFond

IF(@FondId IS NULL) THROW 50001, 'Code Fond inexistant.', 1; 

DELETE INF FROM InformationFondMedium INF INNER JOIN FondMedium FM ON FM.Id = INF.FondMediumId WHERE FM.FondId = @FondId
DELETE FMI FROM FondMediumIndex FMI INNER JOIN FondMedium FM ON FM.Id = FMI.FondMediumId WHERE FM.FondId = @FondId
DELETE FM FROM FondMedium FM WHERE FM.FondId = @FondId

SET @scriptCreate = '
CREATE TABLE '+ @tableTemp +' (
	[Titre] [nvarchar](max) NULL,
	[Type] [int] NOT NULL,
	[TagsString] [nvarchar](max) NULL,
	[FileName] [nvarchar](max) NULL,
	[ThumbNail] [nvarchar](max) NULL ';

SELECT @scriptCreate += COALESCE((
SELECT ', [GI' + CAST(ROW_NUMBER() over(order by GI.Nom) as nvarchar(10)) + '] [nvarchar](max) NULL ' FROM Fond F
INNER JOIN FondGroupeIndex FGI on FGI.FondId = F.Id
INNER JOIN GroupeIndex GI ON FGI.GroupeIndexId = GI.Id
WHERE F.Id = @FondId
ORDER BY GI.Nom
FOR XML PATH('')), '')

SELECT @scriptCreate += COALESCE((
SELECT ', [INFO' + CAST(ROW_NUMBER() over(order by INF.Nom) as nvarchar(10)) + '] [nvarchar](max) NULL ' FROM Fond F
INNER JOIN InformationFond INF ON INF.FondId = F.Id
WHERE F.Id = @FondId
ORDER BY INF.Nom
FOR XML PATH('')), '')

SET @scriptCreate += ' )'



SET @scriptBulk = ' BULK INSERT ' + @tableTemp + '
FROM ''' + @path + '''
WITH
(
	CODEPAGE = ''ACP'',
    FIRSTROW = 2,
    FIELDTERMINATOR = '';'',
    ROWTERMINATOR = ''\n'',   --Use to shift the control to next row
    TABLOCK,
	KEEPNULLS
)';

DECLARE @SeedIdentity INT 
SELECT @SeedIdentity = COALESCE(MAX(Id),100000) FROM FondMedium
DECLARE @insertIds NVARCHAR(MAX) = ' ALTER TABLE ' + @tableTemp + ' 
ADD [Id] [int]; 
UPDATE x
SET x.Id = x.RN
FROM (
      SELECT Id, ROW_NUMBER() OVER (ORDER BY Titre) + ' + CAST(@SeedIdentity as NVARCHAR(10)) + ' AS RN
      FROM ' + @tableTemp + '
      ) x
'

SET @script = @scriptCreate + @scriptBulk + @insertIds + ' SELECT * FROM ' + @tableTemp 


EXEC(@script)


DECLARE @scriptInsert NVARCHAR(MAX) = 'INSERT INTO [dbo].[FondMedium]
           ([Id]
           ,[FondId]
           ,[Titre]
           ,[Type]
           ,[TagsString]
           ,[FileName]
           ,[ThumbNail])
		SELECT [Id]
				,' + CAST( @FondId as nvarchar(10)) + '
				,[Titre]
				,[Type]
				,[TagsString]
				,[FileName]
				,[ThumbNail]
		  FROM ' + @tableTemp;

EXEC(@scriptInsert)

-- DENO INDEX

DECLARE @MaxFontMediumIndex INT
DECLARE @DenoIX NVARCHAR(MAX) 
DECLARE @GroupeIXId INT
DECLARE @ColonneGI NVARCHAR(15)
DECLARE db_cursor CURSOR FOR 
	SELECT GI.Id, '[GI' + CAST(ROW_NUMBER() over(order by GI.Nom) as nvarchar(10)) + ']' FROM Fond F
	INNER JOIN FondGroupeIndex FGI on FGI.FondId = F.Id
	INNER JOIN GroupeIndex GI ON FGI.GroupeIndexId = GI.Id
	WHERE F.Id = @FondId

OPEN db_cursor
FETCH NEXT FROM db_cursor INTO @GroupeIXId, @ColonneGI 

WHILE @@FETCH_STATUS = 0  
BEGIN 

SELECT @MaxFontMediumIndex = COALESCE(MAX(Id),0) FROM FondMediumIndex

SET @DenoIX = ';with cte (IdMedium, IxItem, ' + @ColonneGI + ') as
(
  select 
	Id,
    LTRIM(RTRIM(cast(left(' + @ColonneGI + ', charindex(''|'',' + @ColonneGI + '+''|'')-1) as varchar(50)))) IxItem,
         stuff(' + @ColonneGI + ', 1, charindex(''|'',' + @ColonneGI + '+''|''), '''') ' + @ColonneGI + '
  from ' + @tableTemp + '
  union all
  select 
	IdMedium,
    LTRIM(RTRIM(cast(left(' + @ColonneGI + ', charindex(''|'',' + @ColonneGI + '+''|'')-1) as varchar(50)))) IxItem,
    stuff(' + @ColonneGI + ', 1, charindex(''|'',' + @ColonneGI + '+''|''), '''') ' + @ColonneGI + '
  from cte
  where ' + @ColonneGI + ' > ''''
) 
INSERT INTO FondMediumIndex(id, IndexId, FondMediumId)
select ROW_NUMBER() OVER (order by IX.Id) + ' + CAST(@MaxFontMediumIndex as nvarchar(50)) + ', IX.Id, IdMedium
from cte
INNER JOIN [Index] IX ON IX.Libelle = cte.IxItem
where IxItem IS NOT NULL AND IX.GroupeIndexId = ' + CAST( @GroupeIXId as nvarchar(10))

FETCH NEXT FROM db_cursor INTO @GroupeIXId, @ColonneGI 

EXEC(@DenoIX)

END

CLOSE db_cursor  
DEALLOCATE db_cursor 

-- DENO INFOS

DECLARE @DenoInfo NVARCHAR(MAX) 
DECLARE @InformationFondId INT
DECLARE @TypeColonneInfo INT
DECLARE @ColonneInfo NVARCHAR(15)
DECLARE @MaxInfoMediumID INT
DECLARE db_cursor2 CURSOR FOR 
	SELECT INF.Id, INF.[Type], '[INFO' + CAST(ROW_NUMBER() over(order by INF.Nom) as nvarchar(10)) + ']' FROM Fond F
	INNER JOIN InformationFond INF ON INF.FondId = F.Id
	WHERE F.Id = @FondId
	ORDER BY INF.Nom

OPEN db_cursor2
FETCH NEXT FROM db_cursor2 INTO @InformationFondId, @TypeColonneInfo, @ColonneInfo 

WHILE @@FETCH_STATUS = 0  
BEGIN 

SELECT @MaxInfoMediumID = COALESCE(MAX(Id),0) FROM InformationFondMedium

SET @DenoInfo = 'INSERT INTO [dbo].[InformationFondMedium]
					   ([Id]
					   ,[ValueString]
					   ,[ValueInt]
					   ,[ValueDate]
					   ,[InformationFondId]
					   ,[FondMediumId])
			SELECT ROW_NUMBER() over(ORDER BY Id) + ' + CAST(@MaxInfoMediumID as nvarchar(10)) + ', 
			CASE WHEN ' + CAST(@TypeColonneInfo as nvarchar(10)) + ' = 1 THEN temp.' + @ColonneInfo + '
			ELSE NULL END,
			CASE WHEN ' + CAST(@TypeColonneInfo as nvarchar(10)) + '= 2 THEN temp.' + @ColonneInfo + '
			ELSE NULL END,
			CASE WHEN ' + CAST(@TypeColonneInfo as nvarchar(10)) + ' = 3 THEN temp.' + @ColonneInfo + '
			ELSE NULL END,
			' + CAST(@InformationFondId as nvarchar(10)) + ',
			temp.Id
			FROM ' + @tableTemp + ' temp '




FETCH NEXT FROM db_cursor2 INTO @InformationFondId, @TypeColonneInfo, @ColonneInfo 

EXEC(@DenoInfo)

END

CLOSE db_cursor2  
DEALLOCATE db_cursor2

EXEC(' DROP TABLE ' + @tableTemp )

END
