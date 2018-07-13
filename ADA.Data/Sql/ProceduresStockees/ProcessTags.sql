CREATE PROCEDURE [dbo].[ProcessTags] 
AS 
BEGIN

	
DELETE FROM MediumTag

DELETE FROM Tag

SELECT ID, TagsString INTO #allMedia FROM Medium

INSERT INTO #allMedia ( ID, TagsString )
SELECT ID, TagsString FROM FondMedium

;with cte (IdMedium, TagItem, TagsString) as
(
  select Id,
    cast(left(TagsString, charindex('|',TagsString+'|')-1) as varchar(50)) TagItem,
         stuff(TagsString, 1, charindex('|',TagsString+'|'), '') TagsString
  from #allMedia
  union all
  select IdMedium,
    cast(left(TagsString, charindex('|',TagsString+'|')-1) as varchar(50)) TagItem,
    stuff(TagsString, 1, charindex('|',TagsString+'|'), '') TagsString
  from cte
  where TagsString > ''
) 
select ROW_NUMBER() over(order by IdMedium) as IdTag, IdMedium, TagItem
INTO #tags
from cte
where TagItem IS NOT NULL


INSERT INTO Tag (id, libelle)
select IdTag, TagItem
from #tags


INSERT INTO MediumTag(id, Medium_Id, TagId)
SELECT IdTag,
IdMedium,
IdTag
from #tags




END

