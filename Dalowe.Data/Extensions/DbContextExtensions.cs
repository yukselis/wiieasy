using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Dalowe.Data.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddOrUpdateColumnsDescriptions(this DbContext mydbContext)
        {
            var defaultProperties = typeof(DbContext).GetProperties().Select(pi => pi.Name).ToList();
            var dbSetProperties = mydbContext.GetType().GetProperties().Where(p => !defaultProperties.Contains(p.Name) && p.PropertyType.IsGenericType);

            foreach (var dbSetProperty in dbSetProperties)
            {
                var entityModelType = dbSetProperty.PropertyType.GetGenericArguments().First();
                var propertyInfos = entityModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute<DescriptionAttribute>() != null);

                var tableAttribute = entityModelType.GetCustomAttribute<TableAttribute>();
                var tableName = tableAttribute?.Name ?? dbSetProperty.Name;
                var schemaName = tableAttribute?.Schema ?? "dbo";

                var tableDescription = entityModelType.GetCustomAttribute<DescriptionAttribute>().Description;


                var tablesql = $@"DECLARE @MS_DescriptionValue nvarchar(200);
                                SET @MS_DescriptionValue = N'{tableDescription}';

                                DECLARE @MS_Description nvarchar(200) = NULL;
                                SET @MS_Description = (SELECT
                                       CAST(value AS nvarchar(200)) AS [MS_Description]
                                      FROM
                                       sys.schemas sch
                                       inner join sys.tables t on sch.schema_id = t.schema_id
                                       inner join sys.extended_properties ep on ep.major_id = t.object_id
                                      WHERE
                                       ep.name = N'MS_Description'
                                       AND ep.minor_id = 0
                                       AND sch.name = N'{schemaName}'
                                       AND t.name = N'{tableName}');

                                IF @MS_Description IS NULL
                                BEGIN
                                    EXEC sys.sp_addextendedproperty @name = N'MS_Description',
                                                                    @value = @MS_DescriptionValue,
                                                                    @level0type = N'SCHEMA',
                                                                    @level0name = N'{schemaName}',
                                                                    @level1type = N'TABLE',
                                                                    @level1name = N'{tableName}';
                                END
                                ELSE
                                BEGIN
                                    EXEC sys.sp_updateextendedproperty @name = N'MS_Description',
                                                                        @value = @MS_DescriptionValue,
                                                                        @level0type = N'SCHEMA',
                                                                        @level0name = N'{schemaName}',
                                                                        @level1type = N'TABLE',
                                                                        @level1name = N'{tableName}';
                                END";

                mydbContext.Database.ExecuteSqlCommand(tablesql);

                foreach (var propertyInfo in propertyInfos)
                {
                    var columnName = propertyInfo.GetCustomAttribute<ColumnAttribute>()?.Name ?? propertyInfo.Name;
                    var description = propertyInfo.GetCustomAttribute<DescriptionAttribute>().Description;

                    #region Add or Update MS_Description

                    var sql = $@"DECLARE @MS_DescriptionValue nvarchar(200);
                                SET @MS_DescriptionValue = N'{description}';

                                DECLARE @MS_Description nvarchar(200) = NULL;
                                SET @MS_Description = (SELECT
							                                CAST(value AS nvarchar(200)) AS [MS_Description]
						                                FROM
							                                sys.schemas sch
							                                inner join sys.tables t on sch.schema_id = t.schema_id
							                                inner join sys.columns c on t.object_id = c.object_id
							                                inner join sys.extended_properties ep on c.column_id = ep.minor_id and ep.major_id = t.object_id
						                                WHERE
							                                ep.name = N'MS_Description'
							                                AND sch.name = N'{schemaName}'
							                                AND t.name = N'{tableName}'
							                                AND C.name = N'{columnName}');

                                IF @MS_Description IS NULL
                                BEGIN
                                    EXEC sys.sp_addextendedproperty @name = N'MS_Description',
                                                                    @value = @MS_DescriptionValue,
                                                                    @level0type = N'SCHEMA',
                                                                    @level0name = N'{schemaName}',
                                                                    @level1type = N'TABLE',
                                                                    @level1name = N'{tableName}',
                                                                    @level2type = N'COLUMN',
                                                                    @level2name = N'{columnName}';
                                END
                                ELSE
                                BEGIN
                                    EXEC sys.sp_updateextendedproperty @name = N'MS_Description',
                                                                        @value = @MS_DescriptionValue,
                                                                        @level0type = N'SCHEMA',
                                                                        @level0name = N'{schemaName}',
                                                                        @level1type = N'TABLE',
                                                                        @level1name = N'{tableName}',
                                                                        @level2type = N'COLUMN',
                                                                        @level2name = N'{columnName}';
                                END";

                    mydbContext.Database.ExecuteSqlCommand(sql);

                    #endregion
                }
            }
        }
    }
}