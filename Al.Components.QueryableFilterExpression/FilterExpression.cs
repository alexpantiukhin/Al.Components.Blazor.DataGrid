using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Al.Components.QueryableFilterExpression
{
    /// <summary>
    /// Узел выражения фильтрации queryable-запроса
    /// </summary>
    public class FilterExpression<T>
        where T : class
    {
        readonly static Type StringType = typeof(string);
        readonly static string NameStartWithMethod = nameof(string.StartsWith);
        readonly static string NameEndWithMethod = nameof(string.EndsWith);
        readonly static string NameContainsMethod = nameof(string.Contains);
        readonly static string NameIsNullOrEmptyMethod = nameof(string.IsNullOrEmpty);

        /// <summary>
        /// Уникальное имя свойства типа
        /// </summary>
        public string PropertyName { get; }
        public FilterOperation Operation { get; }
        public object Value { get; }
        FilterExpressionGroupType GroupType { get; }
        public IEnumerable<FilterExpression<T>> GroupFilterExpressions { get; }

        readonly Type Type;

        FilterExpression() { Type = typeof(T); }

        /// <summary>
        /// Создание группы выражений
        /// </summary>
        /// <param name="type">Тип группы</param>
        /// <param name="expressions">Набор выражений</param>
        /// <exception cref="ArgumentNullException">Возникает, если не передать набор или передать пустой</exception>
        public FilterExpression(FilterExpressionGroupType type, IEnumerable<FilterExpression<T>> expressions)
            : this()
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
            : this()
        {
            PropertyName = string.IsNullOrWhiteSpace(columnUniqueName)
                ? throw new ArgumentNullException(nameof(columnUniqueName))
                : columnUniqueName;

            Operation = operation;

            Value = value;
        }

        /// <summary>
        /// Возвращает общее выражение исходя из всех условий дерева выражений
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterExpressionsProperties"></param>
        /// <param name="creatingParameterName">Имя параметра вновь создаваемого выражения</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Expression<Func<T, bool>> GetExpression([NotNull] string creatingParameterName)
        {
            if (string.IsNullOrWhiteSpace(creatingParameterName))
                throw new ArgumentNullException(nameof(creatingParameterName));

            var parameter = Expression.Parameter(Type, creatingParameterName);

            var conditionExpression = GetExpression(parameter);

            return Expression.Lambda<Func<T, bool>>(conditionExpression, parameter);

        }

        public Expression GetExpression([NotNull] ParameterExpression creatingParameter)
        {
            Expression result = null;

            if (PropertyName != null)
            {
                var propertyExpression = Expression.Property(creatingParameter, PropertyName);

                result = GetOperationExpression(Operation, propertyExpression, Value);
            }
            else
            {
                foreach (var item in GroupFilterExpressions)
                {
                    var expressionItem = item.GetExpression(creatingParameter);

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


        static Expression? GetOperationExpression(FilterOperation operation, MemberExpression member, object value)
        {
            Expression? result;

            var constant = Expression.Constant(value);

            var stringTypes = new Type[] { StringType };

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
                    MethodInfo startWithMethod = StringType.GetMethod(NameStartWithMethod, stringTypes);
                    result = Expression.Call(member, startWithMethod, constant);
                    break;
                case FilterOperation.NotStartWith:
                    startWithMethod = StringType.GetMethod(NameStartWithMethod, stringTypes);
                    result = Expression.Not(Expression.Call(member, startWithMethod, constant));
                    break;
                case FilterOperation.EndsWith:
                    MethodInfo endWithMethod = StringType.GetMethod(NameEndWithMethod, stringTypes);
                    result = Expression.Call(member, endWithMethod);
                    break;
                case FilterOperation.NotEndsWith:
                    endWithMethod = StringType.GetMethod(NameEndWithMethod, stringTypes);
                    result = Expression.Not(Expression.Call(member, endWithMethod, constant));
                    break;
                case FilterOperation.Contain:
                    MethodInfo containMethod = StringType.GetMethod(NameContainsMethod, stringTypes);
                    result = Expression.Call(member, containMethod, constant);
                    break;
                case FilterOperation.NotContain:
                    containMethod = StringType.GetMethod(NameContainsMethod, stringTypes);
                    result = Expression.Not(Expression.Call(member, containMethod, constant));
                    break;
                default:
                    result = null;
                    break;
            }

            Console.WriteLine(result);

            return result;
        }

        public static FilterExpression<T> GroupOr(params FilterExpression<T>[] filterExpressions)
        {
            if (filterExpressions == null || filterExpressions.Length == 0)
                throw new ArgumentNullException(nameof(filterExpressions));

            return new FilterExpression<T>(FilterExpressionGroupType.Or, filterExpressions);
        }

        public static FilterExpression<T> GroupAnd(params FilterExpression<T>[] filterExpressions)
        {
            if (filterExpressions == null || filterExpressions.Length == 0)
                throw new ArgumentNullException(nameof(filterExpressions));

            return new FilterExpression<T>(FilterExpressionGroupType.And, filterExpressions);
        }
    }
}
