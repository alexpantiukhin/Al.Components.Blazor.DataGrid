using Al.Components.Blazor.DataGrid.Tests.Data;

using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Data.Tests
{
    public class DataModel_RefreshData_Test
    {
        TestDbContext db => new();

        SimpleSqlDataProvider<User> UserDataProvider => new(db.Users.OrderByDescending(x => x.FirstName));


        [Fact]
        public async Task EmptyRequest_AllItemsOrderDescFName()
        {
            //arrange
            var request = new DataPaginateRequest<User>();
            DataModel<User> dataModel = new(UserDataProvider);

            //act
            await dataModel.RefreshData(request);
            var dbCount = db.Users.Count();
            var modelCount = dataModel.Data.Count();

            //assert
            Assert.True(
                dataModel.Data
                    .Select(x => x.Id)
                    .SequenceEqual(db.Users
                        .OrderByDescending(x => x.FirstName)
                        .Select(x => x.Id)));
        }
    }
}