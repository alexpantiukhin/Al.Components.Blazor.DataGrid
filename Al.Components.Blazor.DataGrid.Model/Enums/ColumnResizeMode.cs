namespace Al.Components.Blazor.DataGrid.Model.Enums
{
    public enum ColumnResizeMode
    {
        /// <summary>
        /// Ширина столбца зафиксирована и не меняется через элемент управления
        /// </summary>
        None,
        /// <summary>
        /// Ширина столбца определяется автоматически исходя из свободного места
        /// </summary>
        Auto,
        /// <summary>
        /// Ширина столбца указывается точно и может меняться через элемент управления
        /// </summary>
        Exactly
    }
}
