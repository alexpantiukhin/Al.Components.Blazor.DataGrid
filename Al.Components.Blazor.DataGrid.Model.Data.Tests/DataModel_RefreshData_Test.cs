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

        static IQueryable<User> BaseQuery => Db.Users.OrderByDescending(x => x.FirstName);

        static SimpleEFDataProvider<User> UserDataProvider =>
            new(BaseQuery);

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
                    .SequenceEqual(BaseQuery.Select(x => x.Id)));
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
                    .SequenceEqual((BaseQuery as IOrderedQueryable<User>)
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
            var dbdata = BaseQuery
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
            var dbdata = BaseQuery
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
            var dbdata = BaseQuery
                .Where(x => EF.Functions.Like(x.FirstName, "%Я%"))
                .ToArray();

            //assert
            Assert.True(
                DataModel.Data
                    .Select(x => x.Id)
                    .SequenceEqual(dbdata.Select(x => x.Id)));
        }

        [Fact]
        public async Task Take2_DataCount2AllCountEqualAllData()
        {
            //arrange
            var request = new DataPaginateRequest<User>
            {
                Take = 2
            };
            DataModel<User> DataModel = new(UserDataProvider, new EFFilterExpressionResolver());


            //act 
            await DataModel.RefreshData(request);
            var dbdataCount = BaseQuery.Count();

            //assert
            Assert.Equal(dbdataCount, DataModel.CountAll);
            Assert.Equal(2, DataModel.Data.Count());
        }

        [Fact]
        public async Task CallOnLoadDataStartOnLoadDataEnd()
        {
            //arrange
            var request = new DataPaginateRequest<User>
            {
                Take = 2
            };
            DataModel<User> DataModel = new(UserDataProvider, new EFFilterExpressionResolver());
            bool callStart = false;
            bool callEnd = false;
            long time = 0;
            DataModel.OnLoadDataStart += (token) => Task.Run(() => callStart = true);
            DataModel.OnLoadDataEnd += (ms, token) =>Task.Run(() => { callEnd = true; time = ms; });

            //act 
            await DataModel.RefreshData(request);
            var dbdataCount = BaseQuery.Count();

            //assert
            Assert.True(callStart);
            Assert.True(callEnd);
            Assert.True(time > 0);
        }
    }
}