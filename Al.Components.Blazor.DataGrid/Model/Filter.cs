using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid.Model
{
    public class Filter
    {
        public string Name { get; set; }
        public FilterExpression Expression { get; set; }

        public Filter()
        {
            Expression = new FilterExpression
            {
                AndExpressions = new List<FilterExpression>
                {
                    new FilterExpression { ColumnUniqueName = "п1", Operation = FilterOperation.Equal, Value = 2},
                    new FilterExpression { ColumnUniqueName = "п2", Operation = FilterOperation.Equal, Value = "А"}
                }
            }.Or(new FilterExpression
            {
                AndExpressions = new List<FilterExpression>
                {
                    new FilterExpression { ColumnUniqueName = "п1", Operation = FilterOperation.Equal, Value = 3},
                    new FilterExpression { ColumnUniqueName = "п2", Operation = FilterOperation.Equal, Value = "Б"}
                }
            }).Or(new FilterExpression
            {
                AndExpressions = new List<FilterExpression>
                {
                    new FilterExpression { ColumnUniqueName = "п2", Operation = FilterOperation.Equal, Value = "С"},
                    new FilterExpression { ColumnUniqueName = "п1", Operation = FilterOperation.Equal, Value = 4}
                        .Or(new FilterExpression { ColumnUniqueName = "п1", Operation = FilterOperation.Equal, Value = 5})
                }
            });
        }

    }
}
