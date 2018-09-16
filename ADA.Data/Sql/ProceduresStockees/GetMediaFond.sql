CREATE PROCEDURE [dbo].[GetMediaFond] @fondId INT, @index [dbo].[ListInt] READONLY, @FondInformationValues FondInformationValue READONLY, 
@pageNumber int, @pageSize int, @searchTerm dbo.WordSearch READONLY
AS 
BEGIN

	DECLARE @nbIndex INT 
	DECLARE @nbFondValueInformation INT 

	SELECT @nbIndex = COUNT(DISTINCT ID) FROM @index
	SELECT @nbFondValueInformation = COUNT(1) FROM @FondInformationValues

	DECLARE @sqlScript nvarchar(MAX)
	DECLARE @sqlMediaCommand nvarchar(MAX)
	DECLARE @sqlScriptCount nvarchar(MAX)
	DECLARE @columnList nvarchar(500)
	DECLARE @whereClause nvarchar(500)

	DECLARE @GroupByIndex nvarchar(500)
	DECLARE @HavingIndex nvarchar(500)
	DECLARE @innerIndex nvarchar(500)

	DECLARE @pagination nvarchar(500)

	SET @pagination = ' ORDER BY FM.ID OFFSET ' + CAST(((@pageNumber - 1) * @pageSize) as nvarchar(50))+ ' ROWS FETCH NEXT ' + CAST(@pageSize as nvarchar(50)) + ' ROWS ONLY'

	-- Informations gzenerique

	DECLARE @InfoInnerClause NVARCHAR(3000) = ''
	DECLARE @InfoWhereClause NVARCHAR(3000) = ''

    IF(@nbFondValueInformation > 0)
    BEGIN
	    SELECT @InfoInnerClause = (
	    SELECT 
		    ' INNER JOIN InformationFondMedium IFM' 
		    + CAST(Inj.InformationFondId as nvarchar(10)) 
		    + ' ON FM.ID = IFM' 
		    + CAST(Inj.InformationFondId as nvarchar(10)) 
		    + '.FondMediumID AND IFM' 
		    + CAST(Inj.InformationFondId as nvarchar(10))
		    + '.InformationFondId = ' + CAST(Inj.InformationFondId as nvarchar(10))
	    FROM @FondInformationValues F
	    INNER JOIN InformationFiltreInjectionFond Inj ON Inj.InformationRechercheFondId = F.Id
	    ORDER BY Inj.InformationFondId
	    FOR XML PATH('')
	    ) 

	
	    SELECT  @InfoWhereClause =  STUFF((
	    SELECT 
		    CASE WHEN F.Value IS NULL THEN ' '
	    ELSE
		    ' AND ( IFM' + CAST(Inj.InformationFondId as nvarchar(10))
		    + CASE WHEN INF.[Type] = 1 THEN '.ValueString ' 
			    WHEN INF.[Type] = 2 THEN '.ValueInt ' 
			    WHEN INF.[Type] = 3 THEN '.ValueDate ' 
			    END

		    + CASE WHEN INJ.Operateur = 1 THEN ' = ' 
			    WHEN INJ.Operateur = 2 THEN ' LIKE ' 
			    WHEN INJ.Operateur = 3 THEN ' < ' 
			    WHEN INJ.Operateur = 4 THEN ' <= ' 
			    WHEN INJ.Operateur = 5 THEN ' > ' 
			    WHEN INJ.Operateur = 6 THEN ' >= ' 
			    WHEN INJ.Operateur = 7 THEN ' <> ' 
			    END

		    + CASE WHEN INJ.Operateur = 2 THEN ' ''%' + F.Value + '%'' ' ELSE ' ''' + F.Value + ''' ' END
		    + ' ) ' END
	    FROM @FondInformationValues F
	    INNER JOIN InformationFiltreInjectionFond Inj ON Inj.InformationRechercheFondId = F.Id
	    INNER JOIN InformationFond INF ON INF.ID = Inj.InformationFondId
	    ORDER BY Inj.InformationFondId
	    FOR XML PATH, TYPE).value(N'.[1]',
     N'nvarchar(max)'), 1, 1, N'')

    END
	-- fin get media Fond
	SET @columnList = 'FM.[Id]'


	SET @whereClause = ' WHERE FM.FondId = ' + CONVERT(varchar(10), @fondId); 

	DECLARE @searchTermClause NVARCHAR(max)
	DECLARE @searchTermClauseTag NVARCHAR(max)

	  SELECT @searchTermClause = ' 1 = 1' + 
	  (
	  SELECT CASE WHEN Wordboundary = 1 THEN ' AND ''#'' + Titre + ''#'' Like ''%[^a-z]' + Libelle + '[^a-z]%'''
		ELSE ' AND Titre Like ''%' + Libelle + '%''' 
		END
	  FROM
	  @searchTerm
	  FOR XML PATH('')
	  )

	  SELECT @searchTermClauseTag = ' 1 = 1' + 
	  (
	  SELECT CASE WHEN Wordboundary = 1 THEN ' AND ''#'' + T.Libelle + ''#'' Like ''%[^a-z]' + Libelle + '[^a-z]%'''
		ELSE ' AND T.Libelle Like ''%' + Libelle + '%''' 
		END
	  FROM
	  @searchTerm
	  FOR XML PATH('')
	  )

	if(@searchTermClause IS NOT NULL AND @searchTermClause != '' ) BEGIN
		SET @whereClause += ' AND ' + @searchTermClause + ' '
		SET @whereClause += ' OR (EXISTS (SELECT 1 FROM Tag T INNER JOIN MediumTag MT ON MT.TagId = T.Id ) '
		SET @whereClause += ' AND EXISTS (SELECT 1 FROM Tag T INNER JOIN MediumTag MT ON MT.TagId = T.Id WHERE ' + @searchTermClauseTag +' AND MT.Medium_Id = FM.Id )) '
	END
	SET @whereClause += @InfoWhereClause
	-- construction script

	SET @sqlMediaCommand  = 'SELECT ' + @columnList +
	' FROM FondMedium FM ' + @InfoInnerClause

	IF(@nbIndex > 0) BEGIN
			SET @innerIndex = ' INNER JOIN FondMediumIndex FMI ON FMI.FondMediumId = FM.Id INNER JOIN @index IX ON IX.ID = FMI.IndexId '

			SET @sqlMediaCommand += @innerIndex;
	END

	SET @sqlMediaCommand += @whereClause;

	IF(@nbIndex > 0) BEGIN
			SET @groupByIndex = ' GROUP BY ' + @columnList 
			SET @havingIndex = ' HAVING COUNT(1) = ' + CAST(@nbIndex as nvarchar(10))

			SET @sqlMediaCommand += @groupByIndex + @HavingIndex;
	END


    SET @sqlScriptCount = 'SELECT COUNT(1) FROM ( ' + @sqlMediaCommand + ' ) X ';

	SET @sqlScript = @sqlMediaCommand + @pagination;
	
	EXEC sp_ExecuteSql @sqlScript, N' @index [dbo].[ListInt] READONLY', @index
	EXEC sp_ExecuteSql @sqlScriptCount, N' @index [dbo].[ListInt] READONLY', @index

END
