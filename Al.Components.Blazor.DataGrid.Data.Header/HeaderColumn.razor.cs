﻿using Al.Collections.Orderable;
using Al.Components.Blazor.DataGrid.Model;
using Al.Components.Blazor.DataGrid.Model.Interfaces;
using Al.Components.Blazor.HandRender;
using Al.Components.Blazor.ResizeComponent;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

#nullable disable

namespace Al.Components.Blazor.DataGrid.Data.Header
{
    public partial class HeaderColumn : HandRenderComponent, IDisposable
    {
        [Parameter]
        [EditorRequired]
        public ResizeAreaAbstract ResizeArea { get; set; }


        [Parameter]
        [EditorRequired]
        public DataGridModel DataGridModel { get; set; }

        [Parameter]
        [EditorRequired]
        public OrderableDictionaryNode<string, ColumnModel> ColumnNode { get; set; }


        ElementReference _element;
        Resize ResizeComponent;
        string _gridTemplateColumns
        {
            get
            {
                var columns = "auto";

                if (ColumnNode.Item.Resizable)
                    columns += " 5px";

                if (ColumnNode.Item.Sortable && ColumnNode.Item.Sort != null)
                    columns += " 5px";

                return columns;
            }
        }

        string _class
        {
            get
            {
                return $"column-header {(ColumnNode.Item.Sortable ? "sortable" : "")} {(_isHeaderOver ? "over" : "")}";
            }
        }

        Type headerComponentType;
        Dictionary<string, object> headerComponentParameters;
        bool _isHeaderOver = false;
        bool _resizing = false;
        double _resizeBorder = 7;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (ColumnNode.Item.HeaderComponentTypeName != null)
            {
                headerComponentType = Type.GetType(ColumnNode.Item.HeaderComponentTypeName);

                if (headerComponentType != null)
                    headerComponentParameters = new()
                    {
                        { "DataGridModel", DataGridModel },
                        { "ColumnModel", ColumnNode },
                    };
            }
            ColumnNode.Item.OnSortChanged += OnSortChangedHandler;
        }

        async Task OnBorder()
        {
            var a = 1;
        }


        public async Task ClickSortHandler()
        {
            //SortDirection? newValue;
            //if (ColumnModel.Sort is null)
            //    newValue = SortDirection.Ascending;
            //else if (ColumnModel.Sort == SortDirection.Ascending)
            //    newValue = SortDirection.Descending;
            //else
            //    newValue = null;

            //await ColumnModel.SortChange(newValue);
        }

        async Task OnMouseMoveHeaderHandler(MouseEventArgs e)
        {
            if (DataGridModel.Columns.Draggable 
                && !_resizing
                && (ResizeComponent.Width - e.OffsetX) > _resizeBorder)
                _isHeaderOver = true;

        }

        async Task OnMouseOutHeaderHandler(MouseEventArgs e)
        {
            _isHeaderOver = false;
        }

        public async Task OnResizeStartHandler(ResizeArgs args)
        {
            _resizing = true;
            await DataGridModel.Columns.ResizeStart(ColumnNode);
        }


        public async Task OnResizeEndHandler(ResizeArgs args)
        {
            _resizing = false;
            //if (DataGridModel.Columns.ResizingColumn != null && args.ClientX != 0)
            //{
            //    var headElementProps = await _jsInteropExtension.GetElementProps(_element);

            await DataGridModel.Columns.ResizeEnd(args.NewWidth);
            //}
        }

        async Task OnSortChangedHandler(CancellationToken cancellationToken = default) => RenderAsync();

        public void Dispose()
        {
            ColumnNode.Item.OnSortChanged += OnSortChangedHandler;
        }
    }
}