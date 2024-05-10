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
    public class HomeAddressControllerTests
    {
        private readonly HomeAddressController _homeAddressController;
        private readonly Mock<IHomeAddressService> _mockHomeAddressService = new();
        Guid acc1id = Guid.NewGuid();
        Guid acc2id = Guid.NewGuid();
        public HomeAddressControllerTests()
            {
            // Initialize the HomeAddressController with a mocked IHomeAddressService
            _homeAddressController = new HomeAddressController(_mockHomeAddressService.Object);
            }

            /// Verifies that GetAll returns StatusCode 200 when categories exist.       
       [Fact]
       public async void GetAll_ShouldReturnStatusCode200_WhenHomeAddressExist()
       {
                // Arrange
            List<HomeAddressResponse> homeAddress = new List<HomeAddressResponse>
            {
                new HomeAddressResponse {
                     Address = "Husum",
                    City = "Copenhagen",
                    PostalCode = "2200",
                    Country = "Danmark",
                    Phone = "+228415799"

                 },
                new HomeAddressResponse {
                    Address = "Gladsaxe",
                    City = "Copenhagen",
                    PostalCode = "2400",
                    Country = "Danmark",
                    Phone = "+228515798"
                     },
                
            };

            _mockHomeAddressService.Setup(x => x.GetAllHomeAddresses()).ReturnsAsync(homeAddress);

                // Act
                var result = await _homeAddressController.GetAll();

                // Assert
                var statusCodeResult = (IStatusCodeActionResult)result;
                Assert.Equal(200, statusCodeResult.StatusCode);
       }


        /// Verifies that GetAll returns StatusCode 204 when no categories exist.

        [Fact]
        public async void GetById_ShouldReturnStatusCode200_WhenDataExists()
        {
            // Arrange
            int homeAddressId = 1;
            HomeAddressResponse homeAddress = new HomeAddressResponse 
            {
                Id=homeAddressId,
                Address = "Gladsaxe",
                City = "Copenhagen",
                PostalCode = "2400",
                Country = "Danmark",
                Phone = "+228515798"

            };

            _mockHomeAddressService.Setup(x => x.GetHomeAddressById(It.IsAny<int>()))
                .ReturnsAsync(homeAddress);

            // Act
            var result = await _homeAddressController.GetById(homeAddressId);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }
        [Fact]
        public async void Update_ShouldReturnStatusCode200_WhenHomeAddressIsSuccessfullyUpdated()
        {
            // Arrange
            HomeAddressRequest updateHomeAddress = new HomeAddressRequest 
            {
                Address = "Gladsaxe",
                City = "Copenhagen",
                PostalCode = "2400",
                Country = "Danmark",
                Phone = "+228515798"
            };
            int homeAddressId = 1;
            HomeAddressResponse homeAddressResponse = new HomeAddressResponse 
            {
                Id=homeAddressId,
                Address = "Gladsaxe",
                City = "Copenhagen",
                PostalCode = "2400",
                Country = "Danmark",
                Phone = "+228515798"
            };

            _mockHomeAddressService.Setup(x => x.UpdateHomeAddress(It.IsAny<int>(), 
                It.IsAny<HomeAddressRequest>())).ReturnsAsync(homeAddressResponse);

            // Act
            var result = await _homeAddressController.Update(homeAddressId, updateHomeAddress);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }
    }
}

