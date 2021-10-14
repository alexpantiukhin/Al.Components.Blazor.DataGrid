using System.Collections.Generic;

namespace Al.Components.Blazor.DataGrid.Model
{
    public class FilterExpression
    {
        public string ColumnUniqueName {  get; set; }    
        public FilterOperation Operation { get; set; }
        public object Value { get; set; }
        public FilterExpression OrExpression { get; set; }
        public IEnumerable<FilterExpression> AndExpressions {  get; set; }

        public FilterExpression Or(FilterExpression expression)
        {
            OrExpression = expression;
            return this;    
        }
        public FilterExpression Or(IEnumerable<FilterExpression> andExpressions)
        {
            AndExpressions = andExpressions;
            return this;
        }
    }
}
