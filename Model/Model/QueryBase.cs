using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Model
{
    public static class ExpressHelp
    {
        public static string GetOperator(this ExpressionType expressionType)
        {
            if (expressionType == ExpressionType.GreaterThan) return ">";
            if (expressionType == ExpressionType.GreaterThanOrEqual) return ">=";
            if (expressionType == ExpressionType.LessThan) return "<";
            if (expressionType == ExpressionType.LessThanOrEqual) return "<=";
            return "=";
        }
    }

    public abstract class QueryBase
    {
        public object OtherData { set; get; }
    }

    //查询对象基类
    public class QueryBase<T>:QueryBase
    {
        public DateTime? StartTime { set; get; }
        public DateTime? EndTime { set; get; }
        public decimal? MinMoney { set; get; }
        public decimal? MaxMoney { set; get; }

        public string GetQueryString()
        {
            return string.Format("S={0},E={1},min={2},max={3}", StartTime, EndTime, MinMoney, MaxMoney);
        }

        //public string GetWhere()
        //{
        //    var result = new StringBuilder();
        //    result.Append(" 1=1 ");
        //    foreach (var condition in _expressions)
        //    {
        //        result.Append(" AND " + GetCondition(condition));
        //    }
        //    return result.ToString();
        //}
      //  private readonly IList<Expression<Func<T, bool>>> _expressions = new List<Expression<Func<T, bool>>>();
        private StringBuilder _condition = new StringBuilder(" 1=1 ");
        public string AddCondition(Expression<Func<T, bool>> func)
        {
            _condition.Append(" AND "+ GetCondition(func));
            return _condition.ToString();
        }
        /// <summary>
        /// 根据动态表达式获得属性名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public  string GetPropertyName(Expression<Func<T, object>> expr)
        {
            var propertyName = string.Empty;
            if (expr.Body is UnaryExpression)
            {
                propertyName = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
            }
            else if (expr.Body is MemberExpression)
            {
                propertyName = ((MemberExpression)expr.Body).Member.Name;
            }
            else if (expr.Body is ParameterExpression)
            {
                propertyName = ((ParameterExpression)expr.Body).Type.Name;
            }
            return propertyName;
        }

        public override string ToString()
        {
             return _condition.ToString();
        }

        public static string GetCondition(Expression<Func<T, bool>> expr)
        {
            if (expr.Body is System.Linq.Expressions.BinaryExpression)
            {
                var condExp = (System.Linq.Expressions.BinaryExpression)expr.Body;
                var left = (System.Linq.Expressions.MemberExpression)condExp.Left;
                var member = left.Member;

                var index = member.Name.LastIndexOf('.');
                var leftOper = member.Name.Substring(index + 1);
                var property = (PropertyInfo) member;

                if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                {
                    leftOper = " ABS(" + leftOper + ")";
                }
                var oper = condExp.NodeType.GetOperator();
                var right = (System.Linq.Expressions.MemberExpression)condExp.Right;
                return string.Format("{0}{1}@{2}", leftOper, oper, right.Member.Name);
            }

            return " 1=1 ";

            //var propertyName = string.Empty;
            //if (expr.Body is UnaryExpression)
            //{
            //    propertyName = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
            //}
            //else if (expr.Body is MemberExpression)
            //{
            //    propertyName = ((MemberExpression)expr.Body).Member.Name;
            //}
            //else if (expr.Body is ParameterExpression)
            //{
            //    propertyName = ((ParameterExpression)expr.Body).Type.Name;
            //}
            //return propertyName;
        }

        public virtual IDbDataParameter[] GetSqlParameters()
        {
            var parms = new List<IDbDataParameter>();
            if (StartTime != null)
                parms.Add(new SqlParameter("@startTime", StartTime));
            if (EndTime != null)
                parms.Add(new SqlParameter("@EndTime", EndTime));
            if (MinMoney != null)
                parms.Add(new SqlParameter("@MinMoney", MinMoney));
            if (MaxMoney != null)
                parms.Add(new SqlParameter("@MaxMoney", MaxMoney));
            return parms.ToArray();
        }
    }
}
