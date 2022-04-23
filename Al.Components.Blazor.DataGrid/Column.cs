using Al.Collections;
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
        OrderableDictionary<string, ColumnModel<T>> ColumnsDictionary { get; set; }


        [Parameter]
        public string UniqueName { get; set; }

        [Parameter]
        public Expression<Func<T, object>> FieldExpression { get; set; }

        [Parameter]
        public bool Visible { get; set; } = true;

        [Parameter]
        public bool Sortable { get; set; } = true;

        [Parameter]
        public int Width { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public SortDirection? Sort { get; set; }

        [Parameter]
        public bool Resizable { get; set; } = true;

        [Parameter]
        public ColumnFixedType FixedType { get; set; }

        [Parameter]
        public bool Draggable { get; set; } = true;

        [Parameter]
        public bool Filterable { get; set; } = true;

        [Parameter]
        public int? Index { get; set; }

        [Parameter]
        public RenderFragment HeaderTemplate { get; set; }


        public ColumnModel<T> Model { get; private set; }

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
                    Width = Width,
                    HeaderTemplate = HeaderTemplate
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
                    Width = Width,
                    HeaderTemplate = HeaderTemplate
                };
            else
                throw new ArgumentException($"{nameof(FieldExpression)} or {nameof(UniqueName)} required");

            ColumnsDictionary.Add(Model.UniqueName, Model, Index);
        }
    }
}
