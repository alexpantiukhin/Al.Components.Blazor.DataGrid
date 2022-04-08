using Al.Collections.QueryableFilterExpression;

using System.Linq.Expressions;

namespace BlazorServer
{
    public class OperationResolver : IOperationExpressionResolver
    {
        const string ParameterName = "x";

        public Expression? GetOperationExpression(FilterOperation operation, MemberExpression member, object value, bool ignoreCase)
        {
            ParameterExpression parameterExpression = Expression.Parameter(member.Type, ParameterName);

            switch (operation)
            {
                case FilterOperation.Equals:

                    break;
                case FilterOperation.NotEquals:
                    break;
                case FilterOperation.MoreOrEqual:
                    break;
                case FilterOperation.LessOrEqual:
                    break;
                case FilterOperation.More:
                    break;
                case FilterOperation.Less:
                    break;
                case FilterOperation.StartsWith:
                    break;
                case FilterOperation.NotStartsWith:
                    break;
                case FilterOperation.EndsWith:
                    break;
                case FilterOperation.NotEndsWith:
                    break;
                case FilterOperation.Contains:
                    break;
                case FilterOperation.NotContains:
                    break;
                default:
                    break;
            }

            return parameterExpression;
        }
    }
}
