using System;
using System.Linq;
using System.Linq.Expressions;

namespace AmazingStore.Domain.Shared.Extensions
{
    public static class ExpressionExtensions
    {
        #region Public Methods

        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector,
            System.ComponentModel.ListSortDirection sortOrder
        )
        {
            if (sortOrder == System.ComponentModel.ListSortDirection.Ascending)
                return source.OrderBy(keySelector);

            return source.OrderByDescending(keySelector);
        }

        #endregion Public Methods

        #region Private Classes

        private class ReplaceExpressionVisitor
                    : ExpressionVisitor
        {

            #region Private Fields

            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            #endregion Private Fields

            #region Public Constructors

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            #endregion Public Constructors

            #region Public Methods

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }

            #endregion Public Methods

        }

        #endregion Private Classes
    }
}