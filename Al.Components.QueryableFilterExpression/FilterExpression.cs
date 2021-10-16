using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Al.Components.QueryableFilterExpression
{
    /// <summary>
    /// Узел выражения фильтрации queryable-запроса
    /// </summary>
    public class FilterExpression
    {
        readonly static Type StringType = typeof(string);
        readonly static string NameStartWithMethod = nameof(string.StartsWith);
        readonly static string NameEndWithMethod = nameof(string.EndsWith);
        readonly static string NameContainsMethod = nameof(string.Contains);
        readonly static string NameIsNullOrEmptyMethod = nameof(string.IsNullOrEmpty);

        /// <summary>
        /// Уникальное имя свойства типа
        /// </summary>
        public string ColumnUniqueName { get; }
        public FilterOperation Operation { get; }
        public object Value { get; }
        FilterExpressionGroupType GroupType { get; }
        public IEnumerable<FilterExpression> GroupFilterExpressions { get; }

        private FilterExpression() { }

        /// <summary>
        /// Создание группы выражений
        /// </summary>
        /// <param name="type">Тип группы</param>
        /// <param name="expressions">Набор выражений</param>
        /// <exception cref="ArgumentNullException">Возникает, если не передать набор или передать пустой</exception>
        public FilterExpression(FilterExpressionGroupType type, IEnumerable<FilterExpression> expressions)
        {
            GroupType = type;

            if (expressions?.Any() != true)
                throw new ArgumentNullException(nameof(expressions), "Expression null or empty");

            // предотвращает преобразование к list с последующей возможностью изменения
            GroupFilterExpressions = expressions.AsEnumerable();
        }

        /// <summary>
        /// Создание выражения
        /// </summary>
        /// <param name="columnUniqueName">Уникальное имя столбца</param>
        /// <param name="operation">Оператор</param>
        /// <param name="value">Значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FilterExpression(string columnUniqueName, FilterOperation operation, object value)
        {
            ColumnUniqueName = string.IsNullOrWhiteSpace(columnUniqueName)
                ? throw new ArgumentNullException(nameof(columnUniqueName))
                : columnUniqueName;

            Operation = operation;

            Value = value;
        }
        
        /// <summary>
        /// Возвращает общее выражение исходя из всех условий дерева выражений
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Expression<Func<T, bool>> GetExpression<T>([NotNull] IEnumerable<IFilterExpressionProperty> columns, [NotNull] string parameterName)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException(nameof(parameterName));

            if (columns?.Any() != true)
                throw new ArgumentNullException(nameof(columns));

            var elementType = typeof(T);

            var parameter = Expression.Parameter(elementType, parameterName);

            var conditionExpression = GetExpression(columns, parameter);

            return Expression.Lambda<Func<T, bool>>(conditionExpression, parameter);

        }

        public Expression GetExpression([NotNull] IEnumerable<IFilterExpressionProperty> columns, [NotNull] ParameterExpression parameter)
        {
            Expression result = null;

            if (ColumnUniqueName != null)
            {
                var column = columns.First(x => x.UniqueName == ColumnUniqueName);

                result = column.MemberExpression;

                var member = column.MemberExpression.Member;

                var propertyExpression = Expression.Property(parameter, member.Name);

                result = GetOperationExpression(Operation, propertyExpression, Value);
            }
            else
            {
                foreach (var item in GroupFilterExpressions)
                {
                    var expressionItem = item.GetExpression(columns, parameter);

                    if (result == null)
                        result = expressionItem;
                    else
                    {
                        if (GroupType == FilterExpressionGroupType.Or)
                            result = Expression.Or(result, expressionItem);
                        else
                            result = Expression.And(result, expressionItem);
                    }
                }
            }

            return result;
        }


        static Expression GetOperationExpression(FilterOperation operation, MemberExpression member, object value)
        {
            Expression result;

            var constant = Expression.Constant(value);

            switch (operation)
            {
                case FilterOperation.Equal:
                    result = Expression.Equal(member, constant);
                    break;
                case FilterOperation.NotEqual:
                    result = Expression.NotEqual(member, constant);
                    break;
                case FilterOperation.MoreOrEqual:
                    result = Expression.GreaterThan(member, constant);
                    break;
                case FilterOperation.LessOrEqual:
                    result = Expression.LessThanOrEqual(member, constant);
                    break;
                case FilterOperation.More:
                    result = Expression.GreaterThan(member, constant);

                    break;
                case FilterOperation.Less:
                    result = Expression.LessThan(member, constant);
                    break;
                case FilterOperation.StartWith:
                    MethodInfo startWithMethod = StringType.GetMethod(NameStartWithMethod, new Type[] { StringType });
                    result = Expression.Call(member, startWithMethod, constant);
                    break;
                case FilterOperation.NotStartWith:
                    startWithMethod = StringType.GetMethod(NameStartWithMethod, new Type[] { StringType });
                    result = Expression.Not(Expression.Call(member, startWithMethod, constant));
                    break;
                case FilterOperation.EndsWith:
                    MethodInfo endWithMethod = typeof(string).GetMethod(NameEndWithMethod, new Type[] { StringType });
                    result = Expression.Call(member, endWithMethod);
                    break;
                case FilterOperation.NotEndsWith:
                    endWithMethod = StringType.GetMethod(NameEndWithMethod, new Type[] { StringType });
                    result = Expression.Not(Expression.Call(member, endWithMethod, constant));
                    break;
                case FilterOperation.Contain:
                    MethodInfo containMethod = StringType.GetMethod(nameof(string.Contains), new Type[] { StringType });
                    result = Expression.Call(member, containMethod, constant);
                    break;
                case FilterOperation.NotContain:
                    containMethod = StringType.GetMethod(nameof(string.Contains), new Type[] { StringType });
                    result = Expression.Not(Expression.Call(member, containMethod, constant));
                    break;
                case FilterOperation.IsNull:
                    constant = Expression.Constant(null);
                    result = Expression.Equal(member, constant);
                    break;
                case FilterOperation.IsNotNull:
                    constant = Expression.Constant(null);
                    result = Expression.NotEqual(member, constant);
                    break;
                case FilterOperation.IsEmpty:
                    constant = Expression.Constant(string.Empty);
                    result = Expression.Equal(member, constant);
                    break;
                case FilterOperation.IsNotEmpty:
                    constant = Expression.Constant(string.Empty);
                    result = Expression.Not(Expression.Equal(member, constant));
                    break;
                case FilterOperation.IsNullOrEmpty:
                    MethodInfo isNullOrEmptyMethod = StringType.GetMethod(nameof(string.IsNullOrEmpty), new Type[] { StringType });
                    result = Expression.Call(member, isNullOrEmptyMethod);
                    break;
                case FilterOperation.IsNotNullOrEmpty:
                    isNullOrEmptyMethod = StringType.GetMethod(nameof(string.IsNullOrEmpty), new Type[] { StringType });
                    result = Expression.Not(Expression.Call(member, isNullOrEmptyMethod));
                    break;
                default:
                    result = null;
                    break;
            }

            Console.WriteLine(result);

            return result;
        }
    }
}
