// See https://aka.ms/new-console-template for more information

namespace Al.Components.Blazor.DataGrid.Tests.Data
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        ICollection<UserBook> UserBooks { get; set; }
    }
}
