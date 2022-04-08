using Al.Collections.QueryableFilterExpression;

namespace Al.Components.Blazor.DataGrid.TestsData
{
    public static class FilterExpressionExtensions
    {
        public static bool Equal<T>(this FilterExpression<T> filter, FilterExpression<T>? compareFilter)
            where T : class
        {
            if (compareFilter is null)
                return false;

            if(filter.PropertyName is null)
            {
                if (filter.GroupType != compareFilter.GroupType)
                    return false;

                if (filter.GroupFilterExpressions.Count() != compareFilter.GroupFilterExpressions.Count())
                    return false;

                foreach (var item in filter.GroupFilterExpressions)
                {
                    if (!compareFilter.GroupFilterExpressions.Any(x => x.Equal(item)))
                        return false;
                }
            }
            else
            {
                if (filter.PropertyName != compareFilter.PropertyName)
                    return false;

                if (filter.Operation != compareFilter.Operation)
                    return false;

                if (filter.IgnoreCase != compareFilter.IgnoreCase)
                    return false;

                if (filter.Value is null && compareFilter.Value is not null
                    || filter.Value is not null && compareFilter.Value is null)
                    return false;

                if(filter.Value is not null && compareFilter.Value is not null)
                {
                    var filterValueString = filter.Value.ToString();
                    var compareFilterValueString = compareFilter.Value.ToString();

                    if (filterValueString != compareFilterValueString)
                        return false;
                }
            }

            return true;
        }
    }
}
