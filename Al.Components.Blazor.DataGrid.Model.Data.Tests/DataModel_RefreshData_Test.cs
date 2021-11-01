using Al.Components.Blazor.DataGrid.Tests.Data;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel;
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

            //assert
            Assert.True(
                dataModel.Data
                    .Select(x => x.Id)
                    .SequenceEqual(db.Users
                        .OrderByDescending(x => x.FirstName)
                        .Select(x => x.Id)));
        }

        [Fact]
        public async Task RequestSortByIdDesct_AllItemsOrderDescFNameAndDescById()
        {
            //arrange
            var request = new DataPaginateRequest<User>();
            request.Sorts.Add(nameof(User.Id), ListSortDirection.Descending);
            DataModel<User> dataModel = new(UserDataProvider);

            //act
            await dataModel.RefreshData(request);

            //assert
            Assert.True(
                dataModel.Data
                    .Select(x => x.Id)
                    .SequenceEqual(db.Users
                        .OrderByDescending(x => x.FirstName)
                        .ThenByDescending(x => x.Id)
                        .Select(x => x.Id)));
        }


        [Fact]
        public async Task RequestSkip3Take2_AllItemsSkip3Take2()
        {
            //arrange
            var request = new DataPaginateRequest<User>();
            request.Skip = 3;
            request.Take = 2;
            DataModel<User> dataModel = new(UserDataProvider);

            //act
            await dataModel.RefreshData(request);
            var dbdata = db.Users
                .OrderByDescending(x => x.FirstName)
                .Skip(3)
                .Take(2)
                .Select(x => x.Id)
                .ToArray();
            //assert
            Assert.True(
                dataModel.Data
                    .Select(x => x.Id)
                    .SequenceEqual(dbdata));
        }

    }
}