using Al.Components.Blazor.AlDataGrid.Model;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Al.Components.Blazor.DataGrid.Model
{
    public class FilterExpression
    {
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

        public Expression GetExpression<T>([NotNull] IEnumerable<ColumnModel<T>> columns, [NotNull] string parameterName)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException(nameof(parameterName));

            if (columns?.Any() != true)
                throw new ArgumentNullException(nameof(columns));

            var elementType = typeof(T);

            var parameter = Expression.Parameter(elementType, parameterName);

            Expression result;

            if (ColumnUniqueName != null)
            {
                var column = columns.FirstOrDefault(x => x.UniqueName == ColumnUniqueName);

                result = column.MemberExpression;

                var member = column.MemberExpression.Member;


            }
            else
            {
                result = null;

                var str = "";

                foreach (var item in GroupFilterExpressions)
                {
                    str += item.GetExpression(columns, parameter).ToString();
                }

            }

            return result;
        }
    }
}
