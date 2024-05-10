using H6_ChicBotique.Controllers;
using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Helpers;
using H6_ChicBotique.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;


namespace H6_ChicBotiqueTestProject.ControllerTests
{

    /// Unit tests for the "AccountInfoController" class.     
    public class AccountInfoControllerTests
    {
        private readonly AccountInfoController _accountInfoController;
        private readonly Mock<IAccountInfoService> _mockAccountInfoService = new();
        Guid acc1id = Guid.NewGuid();
        Guid acc2id = Guid.NewGuid();
        public AccountInfoControllerTests()
            {
            // Initialize the CategoryController with a mocked ICategoryService
            _accountInfoController = new AccountInfoController(_mockAccountInfoService.Object);
            }

            /// Verifies that GetAll returns StatusCode 200 when AccountInfo exist.       
       [Fact]
       public async void GetAllAccountInfos_ShouldReturnStatusCode200_WhenAccountInfoExist()
       {
                // Arrange
            List<AccountInfoResponse> accountInfos = new List<AccountInfoResponse>
            {
                new AccountInfoResponse {
                    Id = acc1id,
                    CreatedDate = DateTime.UtcNow,
                    UserId=1,
                 },
                new AccountInfoResponse {  Id = acc2id,
                    CreatedDate = DateTime.UtcNow,
                    UserId=2, },
                
            };

            _mockAccountInfoService.Setup(x => x.GetAll()).ReturnsAsync(accountInfos);

                // Act
                var result = await _accountInfoController.GetAll();

                // Assert
                var statusCodeResult = (IStatusCodeActionResult)result;
                Assert.Equal(200, statusCodeResult.StatusCode);
       }


        /// Verifies that GetGuidByUserId returns StatusCode 204 when no AccountInfo exist.

        [Fact]
        public async void GetGuidByUserId_ShouldReturnStatusCode200_WhenAccountInfoExist()
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
            _mockAccountInfoService
                  .Setup(x => x.GetGuidIdByUserId(It.IsAny<int>()))
                  .ReturnsAsync(acc1id);

            // Act
            var result = await _accountInfoController.GetUserGuid(1);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);


        }


    }
}

