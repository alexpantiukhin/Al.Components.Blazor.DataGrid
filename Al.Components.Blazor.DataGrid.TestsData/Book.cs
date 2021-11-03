// See https://aka.ms/new-console-template for more information

namespace Al.Components.Blazor.DataGrid.Tests.Data
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        ICollection<UserBook> UserBooks { get; set; }
    }
}
