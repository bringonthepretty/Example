using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Example;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T));
        var expressionVisitor = new ReplaceParameterExpressionVisitor(parameter);

        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                expressionVisitor.Visit(first.Body),
                expressionVisitor.Visit(second.Body)),
            parameter);
    }

    public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T));
        var expressionVisitor = new ReplaceParameterExpressionVisitor(parameter);

        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(
                expressionVisitor.Visit(first.Body),
                expressionVisitor.Visit(second.Body)),
            parameter);
    }

    private class ReplaceParameterExpressionVisitor(Expression newParameter) : ExpressionVisitor
    {
        [return: NotNullIfNotNull("node")]
        public override Expression? Visit(Expression? node)
        {
            if (node is null)
            {
                return null;
            }

            return node.NodeType == ExpressionType.Parameter ? newParameter : base.Visit(node);
        }
    }
}