using System;
using System.Linq.Expressions;

namespace Invoices.Data.Commons
{
    public class Or
    {
        public static Expression<Func<T, bool>> Combine<T>(Func<T, bool> left, Func<T, bool> right)
        {
            if (left == null && right == null) throw new ArgumentException("At least one argument must not be null");
            if (left == null) return Expression.Lambda<Func<T, bool>>(Expression.Call(left.Method));
            if (right == null) return Expression.Lambda<Func<T, bool>>(Expression.Call(right.Method));

            var leftExpression = Expression.Lambda<Func<T, bool>>(Expression.Call(left.Method));
            var rightExpression = Expression.Lambda<Func<T, bool>>(Expression.Call(right.Method));

            var parameter = Expression.Parameter(typeof(T), "p");
            var combined = new ParameterReplacer(parameter).Visit(Expression.OrElse(leftExpression.Body, rightExpression.Body));
            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }

        class ParameterReplacer : ExpressionVisitor
        {
            readonly ParameterExpression parameter;

            internal ParameterReplacer(ParameterExpression parameter)
            {
                this.parameter = parameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return parameter;
            }
        }
    }
}
