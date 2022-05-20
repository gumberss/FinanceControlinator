﻿using System;
using System.Linq.Expressions;

namespace Invoices.Data.Commons
{
    public static class OrExtension
    {
        public static Expression<Func<T, bool>> Or<T>(this Func<T, bool> left, Func<T, bool> right)
        {
            if (left == null && right == null) throw new ArgumentException("At least one argument must not be null");
            if (left == null) return Expression.Lambda<Func<T, bool>>(Expression.Call(left.Method));
            if (right == null) return Expression.Lambda<Func<T, bool>>(Expression.Call(right.Method));

            var leftExpression = ToExpression(left);
            var rightExpression = ToExpression(right);

            var parameter = Expression.Parameter(typeof(T), "p");
            var combined = new ParameterReplacer(parameter).Visit(Expression.OrElse(leftExpression.Body, rightExpression.Body));
            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }

        private static Expression<Func<T, bool>> ToExpression<T>(Func<T, bool> left) => x => left(x);

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
