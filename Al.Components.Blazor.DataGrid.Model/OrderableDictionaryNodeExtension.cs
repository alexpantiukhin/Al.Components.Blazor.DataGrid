using Al.Collections;

namespace Al.Components.Blazor.DataGrid.Model
{
    internal static class OrderableDictionaryNodeExtension
    {
        public static OrderableDictionaryNode<string, ColumnModel<T>>? NextVisible<T>(this OrderableDictionaryNode<string, ColumnModel<T>> node)
            where T : class
            => node.Nexts.FirstOrDefault(x => x.Value.Visible);

    }
}
