CREATE PROCEDURE [dbo].[GetPretreFonctionLieu] @typeLieuId INT = null,  @nomLieu nvarchar(2000) = null,
@lieuId INT = null, @fonctionId INT = null, 
@contextHistoriqueId INT = null, @anneeExercice INT = null, @tri INT, @pageNumber INT = 1,
 @pageSize INT = 10
AS 
BEGIN
 DECLARE @paramDefinition nvarchar(max) = N'@typeLieuId INT,  
 @lieuId INT, 
 @nomLieu nvarchar(2000),
 @fonctionId INT, 
 @contextHistoriqueId INT, 
 @anneeExercice INT,
 @pageNumber INT,
 @pageSize INT',
 @select nvarchar(50) = N'SELECT t.Id FROM ( ',
	  @selectCount nvarchar(50) = N'SELECT count(1) FROM ( ',
	  @subQueryBody nvarchar(max) = N' FROM Pretre P
		LEFT OUTER JOIN PretreFonctionLieu PFL ON PFL.PretreId = P.Id 
		LEFT OUTER JOIN Lieu L ON L.Id = PFL.LieuId 
		LEFT OUTER JOIN PretreContextHistorique PCH ON PCH.PretreId = P.Id 
		WHERE (@typeLieuId IS NULL OR
		(L.TypeLieuId = @typeLieuId
		AND (@lieuId IS NULL OR PFL.LieuId = @lieuId) 
		AND (@nomLieu IS NULL OR L.Nom = @nomLieu)
		AND (@fonctionId IS NULL OR PFL.FonctionId = @fonctionId))
		AND (@anneeExercice IS NULL OR (PFL.AnneeDebut <= @anneeExercice AND PFL.AnneeFin >= @anneeExercice)))
		AND (@contextHistoriqueId IS NULL OR PCH.ContextHistoriqueId = @contextHistoriqueId)
		) t ',
	 @whereClause nvarchar(50) = N'WHERE t.rk = 1 ',
	 @offsetClause nvarchar(200) = N'OFFSET (@pageNumber - 1) * @pageSize ROWS FETCH NEXT @pageSize ROWS ONLY ',
	 @subQuerySelect nvarchar(max) = N'SELECT P.Id, ROW_NUMBER() OVER ( partition by ',	
	 @orderByClause nvarchar(max)	

	 SELECT 
		@orderByClause = CASE WHEN @tri % 2 = 0 THEN N'ORDER BY Tri ASC '
		ELSE N'ORDER BY Tri DESC ' END


	SELECT 
		@subQuerySelect += 
		CASE WHEN @tri = 0 OR @tri = 1 THEN N'P.Id order by p.Id Asc ) AS rk, ISNULL(p.Nom, '''') + '' '' + ISNULL(p.Prenom , '''') as Tri '
		WHEN @tri = 2 THEN N'P.Id order by pfl.AnneeDebut Asc ) AS rk, pfl.AnneeDebut as Tri '
		WHEN @tri = 3 THEN N'P.Id order by pfl.AnneeFin Desc ) AS rk, pfl.AnneeDebut as Tri '
		WHEN @tri = 4 THEN N'P.Id order by pfl.AnneeFin Asc ) AS rk, pfl.AnneeFin as Tri '
		WHEN @tri = 5 THEN N'P.Id order by pfl.AnneeFin Desc ) AS rk, pfl.AnneeFin as Tri '
		ELSE N'ORDER BY Tri DESC ' END
	

	DECLARE @selectQuery nvarchar(max) = @select + @subQuerySelect + @subQueryBody + @whereClause + @orderByClause + @offsetClause
	DECLARE @selectCountQuery nvarchar(max) = @selectCount + @subQuerySelect + @subQueryBody + @whereClause 
 

	EXECUTE sp_executesql @selectQuery, 
		@paramDefinition, 
		@typeLieuId = @typeLieuId, 
		@lieuId = @lieuId, 
		@nomLieu = @nomLieu, 
		@fonctionId = @fonctionId, 
		@contextHistoriqueId = @contextHistoriqueId,
		@anneeExercice = @anneeExercice,
		@pageNumber = @pageNumber,
		@pageSize = @pageSize


	EXECUTE sp_executesql @selectCountQuery, 
		@paramDefinition, 
		@typeLieuId = @typeLieuId, 
		@lieuId = @lieuId, 
		@nomLieu = @nomLieu, 
		@fonctionId = @fonctionId, 
		@contextHistoriqueId = @contextHistoriqueId,
		@anneeExercice = @anneeExercice,
		@pageNumber = @pageNumber,
		@pageSize = @pageSize


END

