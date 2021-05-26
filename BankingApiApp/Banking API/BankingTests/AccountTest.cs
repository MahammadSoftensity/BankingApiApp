using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BankingApp.DTOs;
using BankingApp.Helpers.Params;
using BankingApp.Interfaces;
using BankingApp.Services;
using BankingEntities.Entities;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BankingTests
{
    public class AccountTest 
    {
        private readonly DbContextOptions<DataContext> _dbOptions;
        private readonly Mock<IMapper> _mapMock = new Mock<IMapper>();
        private readonly Mock<ITokenService> _tokenService = new Mock<ITokenService>();


        public AccountTest()
        {
            _dbOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "in-memory")
                .Options;
        }

        [Fact]
        public async Task GetTransferHistory_Success()
        {
            var context = new DataContext(_dbOptions);

            var user = new AppUser()
            {
                Name = "test8",
                Accounts = new List<Account>
                {
                    new Account {Sum = 100},
                    new Account {Sum = 10}
                }
            };

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();

            var userService = new AccountService(context, _mapMock.Object);

            var transferParams = new TransferAmountParams()
            {
                CurrentAccountId = user.Accounts.First().Id,
                TransferAccountId = user.Accounts.Last().Id,
                Sum = 10
            };

            var userResult = await userService.TransferAmount(transferParams, user.Id);

            var transferHistory = await userService.GetTransferHistory(user.Id);

            Assert.False(userResult.Errors.Any());
        }

        [Fact]
        public async Task GetTransferHistory_Unsuccessfully()
        {
            var context = new DataContext(_dbOptions);

            var user1 = new AppUser()
            {
                Name = "test9",
                Accounts = new List<Account>
                {
                    new Account {Sum = 100},
                    new Account {Sum = 10}
                }
            };

            var user2 = new AppUser()
            {
                Name = "test10",
                Accounts = new List<Account>
                {
                    new Account {Sum = 100},
                    new Account {Sum = 10}
                }
            };

            await context.Users.AddAsync(user1);
            await context.Users.AddAsync(user2);

            await context.SaveChangesAsync();

            var userService = new AccountService(context, _mapMock.Object);

            var transferParams = new TransferAmountParams()
            {
                CurrentAccountId = user1.Accounts.First().Id,
                TransferAccountId = user1.Accounts.Last().Id,
                Sum = 10
            };

            var userResult = await userService.TransferAmount(transferParams, user1.Id);

            var transferHistory = await userService.GetTransferHistory(user1.Accounts.First().Id, user2.Id);

            Assert.NotNull(userResult.Result);
        }

        [Fact]
        public async Task GetUserBalance_Success()
        {
            var context = new DataContext(_dbOptions);

            var user = new AppUser()
            {
                Name = "test5",
                Accounts = new List<Account>
                {
                    new Account {Sum = 100},
                    new Account {Sum = 10}
                }
            };

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();

            var userService = new AccountService(context, _mapMock.Object);

            var accountParams = new AccountParams()
            {
                Sum = 10
            };

            var userResult = await userService.GetUserBalance(user.Id);

            Assert.False(userResult.Errors.Any());
        }

        [Fact]
        public async Task GetUserBalance_Unsuccessfully()
        {
            var context = new DataContext(_dbOptions);

            var user1 = new AppUser()
            {
                Name = "test6",
                Accounts = new List<Account>
                {
                    new Account {Sum = 100},
                    new Account {Sum = 10}
                }
            };

            var user2 = new AppUser()
            {
                Name = "test7",
                Accounts = new List<Account>
                {
                    new Account {Sum = 100},
                    new Account {Sum = 10}
                }
            };

            await context.Users.AddAsync(user1);
            await context.Users.AddAsync(user2);

            await context.SaveChangesAsync();

            var userService = new AccountService(context, _mapMock.Object);

            var accountParams = new AccountParams()
            {
                Sum = 10
            };

            var userResult = await userService.GetUserBalance(user2.Id, user1.Id);

            Assert.True(userResult.Errors.Any());
            Assert.Null(userResult.Result);
        }

        [Fact]
        public async Task AddUserAccount_Success()
        {
            var context = new DataContext(_dbOptions);

            var user = new AppUser()
            {
                Name = "test3",
                Accounts = new List<Account>
                {
                    new Account() {Sum = 100},
                    new Account() {Sum = 10}
                }
            };

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();

            var userService = new AccountService(context, _mapMock.Object);

            var accountParams = new AccountParams()
            {
                Sum = 10
            };

            var userResult = await userService.AddUserAccount(accountParams, user.Id);

            Assert.False(userResult.Errors.Any());
            Assert.NotNull(userResult.Result);
        }

        [Fact]
        public async Task AddUserAccount_Unsuccessfully()
        {
            var context = new DataContext(_dbOptions);

            var user = new AppUser()
            {
                Name = "test4",
                Accounts = new List<Account>
                {
                    new Account() {Sum = 100},
                    new Account() {Sum = 10}
                }
            };

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();

            var userService = new AccountService(context, _mapMock.Object);

            var accountParams = new AccountParams()
            {
                Sum = -10
            };

            var userResult = await userService.AddUserAccount(accountParams, user.Id);

            Assert.True(userResult.Errors.Any());
            Assert.Null(userResult.Result);
        }


        [Fact]
        public async Task Transfer_Amount_Success()
        {
            var context = new DataContext(_dbOptions);

            var user = new AppUser()
            {
                Name = "test1",
                Accounts = new List<Account>
                {
                    new Account() {Sum = 100},
                    new Account() {Sum = 10}
                }
            };

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();

            var userService = new AccountService(context,_mapMock.Object);

            var transferParams = new TransferAmountParams()
            {
                CurrentAccountId = user.Accounts.First().Id,
                TransferAccountId = user.Accounts.Last().Id,
                Sum = 10
            };

            var userResult = await userService.TransferAmount(transferParams, user.Id);

            Assert.False(userResult.Errors.Any());
            Assert.NotNull(userResult.Result);
            Assert.Equal(90, user.Accounts.First().Sum);
            Assert.Equal(20, user.Accounts.Last().Sum);
        }

        [Fact]
        public async Task Transfer_Amount_Unsuccessfully()
        {
            var context = new DataContext(_dbOptions);

            var user = new AppUser()
            {
                Name = "test2",
                Accounts = new List<Account>
                {
                    new Account() {Sum = 100},
                    new Account() {Sum = 10}
                }
            };

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();

            var userService = new AccountService(context, _mapMock.Object);

            var transferParams = new TransferAmountParams()
            {
                CurrentAccountId = user.Accounts.First().Id,
                TransferAccountId = user.Accounts.Last().Id,
                Sum = -10
            };

            var userResult = await userService.TransferAmount(transferParams, user.Id);

            Assert.True(userResult.Errors.Any());
            Assert.Null(userResult.Result);
        }
    }
}
