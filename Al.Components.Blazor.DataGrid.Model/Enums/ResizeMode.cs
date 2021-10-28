namespace Al.Components.Blazor.DataGrid.Model.Enums
{
    /// <summary>
    /// Режим изменения ширины столбца
    /// </summary>
    public enum ResizeMode
    {
        /// <summary>
        /// За счёт изменения размера всего грида. Если он столбцы не умещаются, то появляется скролл
        /// </summary>
        Table,
        /// <summary>
        /// За счёт изменения размера соседнего столбца, при этом ширину таблицы изменением столбца не изменить
        /// </summary>
        Sibling
    }
}
