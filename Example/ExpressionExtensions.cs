using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Example;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T));
        var expressionVisitor1 = new ReplaceParameterExpressionVisitor(first.Parameters[0], parameter);
        var expressionVisitor2 = new ReplaceParameterExpressionVisitor(second.Parameters[0], parameter);

        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                expressionVisitor1.Visit(first.Body),
                expressionVisitor2.Visit(second.Body)),
            parameter);
    }

    public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T));
        var expressionVisitor1 = new ReplaceParameterExpressionVisitor(first.Parameters[0], parameter);
        var expressionVisitor2 = new ReplaceParameterExpressionVisitor(second.Parameters[0], parameter);

        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(
                expressionVisitor1.Visit(first.Body),
                expressionVisitor2.Visit(second.Body)),
            parameter);
    }

    private class ReplaceParameterExpressionVisitor(Expression oldParameter, Expression newParameter)
        : ExpressionVisitor
    {
        [return: NotNullIfNotNull("node")]
        public override Expression? Visit(Expression? node)
        {
            return node == oldParameter ? newParameter : base.Visit(node);
        }
    }
}