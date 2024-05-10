using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Helpers;
using H6_ChicBotique.Repositories;
using H6_ChicBotique.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H6_ChicBotiqueTestProject.ServiceTests
{
    public class AccountInfoServiceTests
    {
        private readonly AccountInfoService _accountInfoService;
        private readonly Mock<IAccountInfoRepository> _mockAccountInfoRepository = new();
        private readonly Mock<IUserRepository> _mockUserRepository = new();

        Guid acc1id = Guid.NewGuid();
        Guid acc2id = Guid.NewGuid();

        public AccountInfoServiceTests()
        {
            // Initializing the AccountInfoService with the mock accountInfo repository
            _accountInfoService = new AccountInfoService(_mockAccountInfoRepository.Object);
        }
        //Test for GetAllAccountInfo method of service
        [Fact]
        public async void GetAllAccountInfo_ShouldReturnListOfAccountInfoResponses_WhenAccountInfoExist()
        {
            // Arrange
            // Creating a list of accountInfo
            List<AccountInfo> accountInfos = new List<AccountInfo>(); ;


            User newUser = new()
            {
                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",
                Role = Role.Administrator

            };

            accountInfos.Add(new AccountInfo
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,
                UserId=1,
                User=newUser

            });


            // Setting up the mock accountInfo repository to return the list of accountInfo
            _mockAccountInfoRepository
                .Setup(x => x.SelectAll())
                .ReturnsAsync(accountInfos);

            // Act
            var result = await _accountInfoService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.IsType<List<AccountInfoResponse>>(result);
        }
        // Test case for GetAccountInfoById method when AccountInfo exists
        [Fact]
        public async void GetAccountInfoById_ShouldReturnAccountInfoResponse_WhenAccountInfoExists()
        {
            // Arrange
            int productId = 1;
            User newUser = new()
            {
                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",
                Role = Role.Administrator

            };

            AccountInfo accountInfo = new AccountInfo
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,
                UserId=1,
                User=newUser

            };
            _mockAccountInfoRepository
                .Setup(x => x.SelectById(It.IsAny<Guid>()))
                .ReturnsAsync(accountInfo);

            // Act
            var result = await _accountInfoService.GetById(acc1id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AccountInfoResponse>(result);
            Assert.Equal(accountInfo.Id, result.Id);
            Assert.Equal(accountInfo.CreatedDate, result.CreatedDate);
            Assert.Equal(accountInfo.UserId, result.UserId);
           
        }
         

        // Test case for GetAccountInfoById method when AccountInfo does not exist
        [Fact]
        public async void GetAccountInfoById_ShouldReturnNull_WhenAccountInfoDoesNotExist()
        {
            // Arrange
    

            _mockAccountInfoRepository
                .Setup(x => x.SelectById(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _accountInfoService.GetById(acc1id);

            // Assert
            Assert.Null(result);
        }
        // Test case for GetAccountInfoById method when product exists
        [Fact]
        public async void GetGuidByUserId_ShouldReturnGuidIdResponse_WhenAccountInfoExists()
        {
            // Arrange
            int userId = 1;
            User newUser = new()
            {
                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",
                Role = Role.Administrator

            };

            AccountInfo accountInfo = new AccountInfo
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,
                UserId=1,
                User=newUser

            };
            _mockAccountInfoRepository
                  .Setup(x => x.SelectGuidByUserId(It.IsAny<int>()))
                  .ReturnsAsync(acc1id);

            // Act
            var result = await _accountInfoService.GetGuidIdByUserId(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Guid>(result);
            Assert.Equal(accountInfo.Id, result);
           

        }


    }

}


