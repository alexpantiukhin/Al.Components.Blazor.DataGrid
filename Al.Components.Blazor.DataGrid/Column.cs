using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Enums;
using Al.Components.Blazor.DataGrid.Model.Interfaces;
using Al.Components.Blazor.HandRender;

using Microsoft.AspNetCore.Components;

using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Al.Components.Blazor.DataGrid
{
    public partial class Column<T> : HandRenderComponent, IColumn<T>
        where T : class
    {
        [CascadingParameter]
        DataGridModel<T> DataGridModel { get; set; }


        [Parameter]
        public string UniqueName { get; set; }

        [Parameter]
        public Expression<Func<T, object>> FieldExpression { get; set; }

        [Parameter]
        public bool Visible { get; set; }

        [Parameter]
        public bool Sortable { get; set; }

        [Parameter]
        public int Width { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public ListSortDirection? Sort { get; set; }

        [Parameter]
        public bool Resizable { get; set; }

        [Parameter]
        public ColumnFixedType FixedType { get; set; }

        [Parameter]
        public bool Draggable { get; set; }

        [Parameter]
        public bool Filterable { get; set; }

        [Parameter]
        public int? Index { get; set; }

        internal ColumnModel<T> Model { get; private set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (FieldExpression != null)
                Model = new ColumnModel<T>(FieldExpression)
                {
                    Filterable = Filterable,
                    FixedType = FixedType,
                    Resizable = Resizable,
                    Sort = Sort,
                    Sortable = Sortable,
                    Title = Title,
                    Visible = Visible,
                    Width = Width
                };
            else if (UniqueName != null)
                Model = new ColumnModel<T>(UniqueName)
                {
                    Filterable = Filterable,
                    FixedType = FixedType,
                    Resizable = Resizable,
                    Sort = Sort,
                    Sortable = Sortable,
                    Title = Title,
                    Visible = Visible,
                    Width = Width
                };
            else
                throw new ArgumentException($"{nameof(FieldExpression)} or {nameof(UniqueName)} required");

            DataGridModel.Columns.All.Add(Model.UniqueName, Model, Index);
        }
    }
}
