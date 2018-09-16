namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutIndexFullText : DbMigration
    {
        public override void Up()
        {
            Sql(@"Declare @script nvarchar(max)
                    DECLARE @tables TABLE(
                        tableName nvarchar(250),
                        cols nvarchar(2500));

                                INSERT INTO @tables VALUES('ArticleRevue', 'Titre')
                    INSERT INTO @tables VALUES('Bibliotheque', 'Titre')
                    INSERT INTO @tables VALUES('Catalogue', 'Titre')
                    INSERT INTO @tables VALUES('Medium', 'Titre')
                    INSERT INTO @tables VALUES('FondMedium', 'Titre')

                    SELECT @script =
                    (SELECT 'CREATE FULLTEXT INDEX ON [' + KU.table_name + '](' + T.cols + ') ' +
                       'KEY INDEX [' + tc.CONSTRAINT_NAME + '] ' +
                       ' WITH STOPLIST = OFF, CHANGE_TRACKING OFF, NO POPULATION; ' +
                       ''
                    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
                    INNER JOIN
                        INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU
                              ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND
                                 TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME
                    INNER JOIN @tables T ON T.tableName = KU.TABLE_NAME
                    FOR XML PATH(''))

                    EXEC(@script)

SELECT @script = 
(SELECT 'ALTER FULLTEXT INDEX ON [' + T.tableName + '] START FULL POPULATION ;'
FROM @tables T
FOR XML PATH(''))

EXEC(@script)
", true);


        }
        
        public override void Down()
        {
             Sql(@"Declare @script nvarchar(max)
                    DECLARE @tables TABLE(
                        tableName nvarchar(250),
                        cols nvarchar(2500));

                                INSERT INTO @tables VALUES('ArticleRevue', 'Titre')
                    INSERT INTO @tables VALUES('Bibliotheque', 'Titre')
                    INSERT INTO @tables VALUES('Catalogue', 'Titre')
                    INSERT INTO @tables VALUES('Medium', 'Titre')
                    INSERT INTO @tables VALUES('FondMedium', 'Titre')

SELECT @script = 
(SELECT 'DROP FULLTEXT INDEX ON [' + T.tableName + ']; '
FROM  @tables T 
FOR XML PATH(''))


                    EXEC(@script)
", true);


        }
    }
}
