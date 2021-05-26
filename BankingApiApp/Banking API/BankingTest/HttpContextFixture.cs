using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BankingApp.Interfaces;
using BankingEntities.Entities;
using DataAccessLayer.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;

namespace BankingTest
{
    public class HttpContextFixture
    {
        public Mock<IUserService> userService { get; private set; } = new Mock<IUserService>();
        public Mock<IAccountService> accountService { get; private set; } = new Mock<IAccountService>();
        public Mock<DataContext> dbContext { get; private set; } = new Mock<DataContext>();
        public Mock<IMapper> mapperMock { get; private set; } = new Mock<IMapper>();
        public string Token { get; set; }




    }
}
