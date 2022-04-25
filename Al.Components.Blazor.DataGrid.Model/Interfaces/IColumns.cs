using Al.Components.Blazor.DataGrid.Model.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Al.Components.Blazor.DataGrid.Model.Interfaces
{
    public interface IColumns
    {
        bool Draggable { get; }
        ResizeMode ResizeMode { get; }
        bool AllowResizeLastColumn { get; }

    }
}
