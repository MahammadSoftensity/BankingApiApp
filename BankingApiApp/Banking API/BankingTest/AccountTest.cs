using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BankingEntities.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace BankingTest
{
    public class AccountTest : IClassFixture<HttpContextFixture>
    {
        private readonly HttpContextFixture _fixture;

        public AccountTest(HttpContextFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        private async Task Register()
        {
            using var hmac = new HMACSHA512();

            var user1 = new AppUser
            {
                Name = "test1",
                Accounts = new List<Account>
                {
                    new Account{ Sum = 10 },
                    new Account{ Sum = 20 },
                },
                PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("test1")),
                PassSalt = hmac.Key
            };

            var user2 = new AppUser()
            {
                Name = "test2",
                Accounts = new List<Account>
                {
                    new Account{ Sum = 100 },
                    new Account{ Sum = 200 },
                    new Account{ Sum = 5 },
                },
                PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("test2")),
                PassSalt = hmac.Key
            };

           

            _fixture.dbContext.Setup(x => x.Users.Add(user1));
            _fixture.dbContext.Setup(x => x.Users.Add(user2));
            _fixture.dbContext.Setup(x => x.SaveChanges());


        }
    }
}
