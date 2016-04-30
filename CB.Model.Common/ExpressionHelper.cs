using System;
using System.Linq.Expressions;
using System.Reflection;


namespace CB.Model.Common
{
    public static class ExpressionHelper
    {
        public static PropertyInfo GetPropertyInfo(this LambdaExpression propertyExpression)
        {
            if (propertyExpression == null) throw new ArgumentNullException(nameof(propertyExpression));

            var memberExpr = propertyExpression.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException($"{propertyExpression} refers to a method, not a property.");

            var propInfo = memberExpr.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"{propertyExpression} refers to a field, not a property.");

            return propInfo;
        }
    }
}