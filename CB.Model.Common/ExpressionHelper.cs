using System;
using System.Linq.Expressions;
using System.Reflection;


namespace CB.Model.Common
{
    public static class ExpressionHelper
    {
        #region Methods
        public static PropertyInfo GetPropertyInfo(this LambdaExpression propertyExpression)
        {
            if (propertyExpression == null) throw new ArgumentNullException(nameof(propertyExpression));

            var memberExpr = propertyExpression.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException($"{propertyExpression} refers to a method, not a property.");

            var propInfo = memberExpr.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"{propertyExpression} refers to a field, not a property.");

            if (propInfo.GetMethod.IsStatic)
                throw new ArgumentException(
                    $"{propertyExpression} refers to a static property, not an instance property.");

            return propInfo;
        }

        public static string GetPropertyName(this LambdaExpression propertyExpression)
            => GetPropertyInfo(propertyExpression).Name;
        #endregion
    }
}