
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CommonTools
{
    public static class ReflectionHelper<T>
    {
        /// <summary>
        /// 表态式辅助
        /// </summary>
        public delegate TProperty PropertyGetterDelegate<out TProperty>(T target);

        /// <summary>
        ///     Gets property info out of a Lambda.
        /// </summary>
        /// <typeparam name="TProperty">The return type of the Lambda.</typeparam>
        /// <param name="expression">The Lambda expression.</param>
        /// <returns>The property info.</returns>
        public static PropertyInfo GetPropertyInfo<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException("Expression is not a member expression.");
            }
            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new InvalidOperationException("Expression is not for a property.");
            }
            return propertyInfo;
        }
    }

}
