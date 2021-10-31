using Al.Components.Blazor.DataGrid.Tests.Data;

using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Data.Tests
{
    public class DataModel_RefreshData_Test
    {
        TestDbContext db => new ();

        SimpleSqlDataProvider<User> UserDataProvider => new (db.Users);


        [Fact]
        public async Task EmptyRequest_AllItems()
        {
            //arrange
            var request = new DataPaginateRequest<User>();
            DataModel<User> dataModel = new(UserDataProvider);

            //act
            await dataModel.RefreshData(request);
            var dbCount = db.Users.Count();
            var modelCount = dataModel.Data.Count();

            //assert
            Assert.Equal(dbCount, modelCount);
        }
    }
}