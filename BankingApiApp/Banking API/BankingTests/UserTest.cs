using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BankingApp.Helpers.Params;
using BankingApp.Interfaces;
using BankingApp.Services;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BankingTests
{
    public class UserTest
    {
        private readonly DbContextOptions<DataContext> _dbOptions;
        private readonly Mock<IMapper> _mapMock = new Mock<IMapper>();
        private readonly Mock<ITokenService> _tokenService = new Mock<ITokenService>();

        public UserTest()
        {
            _dbOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "in-memory")
                .Options;
        }

        [Fact]
        public async Task Login_Successfully()
        {
            var context = new DataContext(_dbOptions);

            var userService = new UserService(context, _mapMock.Object, _tokenService.Object);

            //Act

            var userParams = new UserParams
            {
                Name = "test2",
                Password = "test2",
                Sum = 100
            };

            var register = await userService.Register(userParams);

            var loginParams = new LoginParams
            {
                Name = "test2",
                Password = "test2"
            };

            var login = await userService.Login(loginParams);

            //Assert

            Assert.False(login.Errors?.Any());
            Assert.NotNull(login.Result);
        }


        [Fact]
        public async Task Login_Unsuccessfully()
        {
            var context = new DataContext(_dbOptions);

            var userService = new UserService(context, _mapMock.Object, _tokenService.Object);

            //Act

            var userParams = new UserParams
            {
                Name = "test3",
                Password = "test3",
                Sum = 100
            };

            var register = await userService.Register(userParams);

            var loginParams = new LoginParams
            {
                Name = "test3",
                Password = "123"
            };

            var login = await userService.Login(loginParams);

            //Assert

            Assert.True(login.Errors?.Any());
            Assert.Null(login.Result);
        }

        [Fact]
        public async Task Register_Successfully()
        {
            var context = new DataContext(_dbOptions);

            var userService = new UserService(context, _mapMock.Object, _tokenService.Object);

            //Act

            var userParams = new UserParams
            {
                Name = "test1",
                Password = "test1",
                Sum = 100
            };

            var result = await userService.Register(userParams);

            //Assert

            Assert.False(result.Errors.Any());
            Assert.NotNull(result.Result);
        }


        [Fact]
        public async Task Register_Unsuccessfully()
        {
            var context = new DataContext(_dbOptions);

            var userService = new UserService(context, _mapMock.Object, _tokenService.Object);

            //Act

            var userParams = new UserParams
            {
                Name = "test",
                Password = "test",
                Sum = -1
            };

            var result = await userService.Register(userParams);

            //Assert

            Assert.True(result.Errors?.Any());
            Assert.Null(result.Result);
        }
    }
}
