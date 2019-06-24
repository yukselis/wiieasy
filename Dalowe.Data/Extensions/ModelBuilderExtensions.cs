using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Dalowe.Domain.Base;

namespace Dalowe.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void HasQueryFilterForAllEntities(this DbModelBuilder modelBuilder, DbContext mydbContext)
        {
            var defaultProperties = typeof(DbContext).GetProperties().Select(pi => pi.Name).ToList();
            var dbSetProperties = mydbContext.GetType().GetProperties().Where(p => !defaultProperties.Contains(p.Name) && p.PropertyType.IsGenericType);

            foreach (var dbSetProperty in dbSetProperties)
            {
                var dbSetType = dbSetProperty.PropertyType;
                if (dbSetType.IsGenericType && dbSetType.GenericTypeArguments.Any(x => x.GetInterfaces().Contains(typeof(IEntity))))
                {
                    var entityType = dbSetProperty.PropertyType.GenericTypeArguments[0];

                    var parameter = Expression.Parameter(entityType, "x");
                    var memberExpression = Expression.Property(parameter, entityType.GetProperty("IsActive", BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException());
                    var lambda = Expression.Lambda(memberExpression, parameter);

                    var queryMethod = modelBuilder.GetType().GetMethods().First(x => x.Name == "Entity" && x.IsGenericMethod);
                    queryMethod = queryMethod.MakeGenericMethod(entityType);
                    var queryBuilder = queryMethod.Invoke(modelBuilder, null);

                    var hasQueryFilterMethod = queryBuilder.GetType().GetMethod("HasQueryFilter", new[] { typeof(LambdaExpression) });
                    hasQueryFilterMethod.Invoke(queryBuilder, new object[] { lambda });
                }
            }
        }

        public static void HasQueryFilterByBaseEntity<T>(this DbModelBuilder modelBuilder, DbContext mydbContext, Expression<Func<T, bool>> filter)
        {
            // TODO: Expression visitor eklenip base entity tipinin expression tree'de değiştirilmesi gerek. Aksi takdirde kullanıma açık olmamalı

            var defaultProperties = typeof(DbContext).GetProperties().Select(pi => pi.Name).ToList();
            var dbSetProperties = mydbContext.GetType().GetProperties().Where(p => !defaultProperties.Contains(p.Name) && p.PropertyType.IsGenericType);

            foreach (var dbSetProperty in dbSetProperties)
            {
                var dbSetType = dbSetProperty.PropertyType;
                if (dbSetType.IsGenericType && dbSetType.GenericTypeArguments.Any(x => x.GetInterfaces().Contains(typeof(T))))
                {
                    var entityType = dbSetProperty.PropertyType.GenericTypeArguments[0];

                    var queryMethod = modelBuilder.GetType().GetMethods().First(x => x.Name == "Entity" && x.IsGenericMethod);
                    queryMethod = queryMethod.MakeGenericMethod(entityType);
                    var queryBuilder = queryMethod.Invoke(modelBuilder, null);

                    var hasQueryFilterMethod = queryBuilder.GetType().GetMethod("HasQueryFilter", new[] { typeof(LambdaExpression) });

                    var parameter = Expression.Parameter(entityType, filter.Parameters[0].Name);
                    var lambda = Expression.Lambda(filter.Body, parameter);

                    hasQueryFilterMethod.Invoke(queryBuilder, new object[] { lambda });
                }
            }
        }
    }
}