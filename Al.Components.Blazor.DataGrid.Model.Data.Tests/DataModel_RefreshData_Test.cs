using Al.Collections.EF.FilterExpressionResolver;
using Al.Collections.QueryableFilterExpression;
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
        static TestDbContext Db => new();

        static SimpleEFDataProvider<User> UserDataProvider =>
            new(Db.Users.OrderByDescending(x => x.FirstName));

        [Fact]
        public async Task EmptyRequest_AllItemsOrderDescFName()
        {
            //arrange
            var request = new DataPaginateRequest<User>();
            DataModel<User> DataModel = new(UserDataProvider, new EFFilterExpressionResolver());

            //act
            await DataModel.RefreshData(request);

            //assert
            Assert.True(
                DataModel.Data
                    .Select(x => x.Id)
                    .SequenceEqual(Db.Users
                        .OrderByDescending(x => x.FirstName)
                        .Select(x => x.Id)));
        }

        [Fact]
        public async Task RequestSortByIdDesct_AllItemsOrderDescFNameAndDescById()
        {
            //arrange
            var request = new DataPaginateRequest<User>();
            request.Sorts.Add(nameof(User.Id), ListSortDirection.Descending);
            DataModel<User> DataModel = new(UserDataProvider, new EFFilterExpressionResolver());

            //act
            await DataModel.RefreshData(request);

            //assert
            Assert.True(
                DataModel.Data
                    .Select(x => x.Id)
                    .SequenceEqual(Db.Users
                        .OrderByDescending(x => x.FirstName)
                        .ThenByDescending(x => x.Id)
                        .Select(x => x.Id)));
        }


        [Fact]
        public async Task RequestSkip3Take2_AllItemsSkip3Take2()
        {
            //arrange
            var request = new DataPaginateRequest<User>
            {
                Skip = 3,
                Take = 2
            };
            DataModel<User> DataModel = new(UserDataProvider, new EFFilterExpressionResolver());

            //act
            await DataModel.RefreshData(request);
            var dbdata = Db.Users
                .OrderByDescending(x => x.FirstName)
                .Skip(3)
                .Take(2)
                .Select(x => x.Id)
                .ToArray();

            //assert
            Assert.True(
                DataModel.Data
                    .Select(x => x.Id)
                    .SequenceEqual(dbdata));
        }

        [Fact]
        public async Task RequestFilterEqualVasya_AllItemsEqualVasya()
        {
            //arrange
            var request = new DataPaginateRequest<User>
            {
                FilterExpression = new(nameof(User.FirstName), FilterOperation.Equals, "Вася")
            };
            DataModel<User> DataModel = new(UserDataProvider, new EFFilterExpressionResolver());

            //act
            await DataModel.RefreshData(request);
            var dbdata = Db.Users
                .OrderByDescending(x => x.FirstName)
                .Where(x => x.FirstName == "Вася")
                .Select(x => x.Id)
                .ToArray();

            //assert
            Assert.True(
                DataModel.Data
                    .Select(x => x.Id)
                    .SequenceEqual(dbdata));
        }

        [Fact]
        public async Task RequestFilterContainsYa_AllItemsContainsYa()
        {
            //arrange
            var request = new DataPaginateRequest<User>
            {
                FilterExpression = new(nameof(User.FirstName), FilterOperation.Contains, "Я")
            };
            DataModel<User> DataModel = new(UserDataProvider, new EFFilterExpressionResolver());

            //act
            await DataModel.RefreshData(request);
            var dbdata = Db.Users
                .OrderByDescending(x => x.FirstName)
                .Where(x => EF.Functions.Like(x.FirstName, "%Я%"))
                .ToArray();

            //assert
            Assert.True(
                DataModel.Data
                    .Select(x => x.Id)
                    .SequenceEqual(dbdata.Select(x => x.Id)));
        }

    }
}