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
    public class ShippingDetailsServiceTests
    {
        private readonly ShippingDetailsService _shippingDetailsService;
        private readonly Mock<IShippingDetailsRepository> _mockShippingDetailsRepository = new();
        private readonly Mock<IOrderRepository> _mockOrderRepository = new();

        Guid acc1id = Guid.NewGuid();
        Guid acc2id = Guid.NewGuid();

        public ShippingDetailsServiceTests()
        {
            // Initializing the AccountInfoService with the mock accountInfo repository
            _shippingDetailsService = new ShippingDetailsService(_mockShippingDetailsRepository.Object);
        }
        //Test for GetAllShippingDetails method of service
        [Fact]
        public async void GetAllShippingDetails_ShouldReturnListOfShippingDetailsResponses_WhenShippingDetailsExist()
        {
            // Arrange
            // Creating a list of ShippingDetails
            List<ShippingDetails> shippingDetails = new List<ShippingDetails>(); ;



            Order order = new Order
            {
                OrderDate = DateTime.Now,
                AccountInfoId =acc1id,
               


            };

            shippingDetails.Add(new ShippingDetails
            {
                
                Id = 1,

                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                Phone = "+228415799",
                Order = order

            });


            // Setting up the mock accountInfo repository to return the list of accountInfo
            _mockShippingDetailsRepository
                .Setup(x => x.SelectAll())
                .ReturnsAsync(shippingDetails);

            // Act
            var result = await _shippingDetailsService.GetAllShippingDetails();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.IsType<List<ShippingDetailsResponse>>(result);
        }
        // Test case for GetShippingDetailsById method when ShippingDetails exists
        [Fact]
        public async void GetShippingDetailsById_ShouldReturnShippingDetailsResponse_WhenShippingDetailsExists()
        {
            // Arrange
            int shippingDetailsId = 1;
            Order order = new Order
            {
                Id=1,
                OrderDate = DateTime.Now,
                AccountInfoId =acc1id,



            };

            ShippingDetails shippingDetails = new ShippingDetails
            {
            
                Id = shippingDetailsId,
                OrderId = order.Id,
                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                Phone = "+228415799",
                Order=order

            };
            _mockShippingDetailsRepository
                .Setup(x => x.SelectById(It.IsAny<int>()))
                .ReturnsAsync(shippingDetails);

            // Act
            var result = await _shippingDetailsService.GetShippingDetailsById(shippingDetailsId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShippingDetailsResponse>(result);
            Assert.Equal(shippingDetails.Id, result.Id);
            Assert.Equal(shippingDetails.Address, result.Address);
            Assert.Equal(shippingDetails.City, result.City);
            Assert.Equal(shippingDetails.PostalCode, result.PostalCode);
            Assert.Equal(shippingDetails.Country, result.Country);
            Assert.Equal(shippingDetails.Phone, result.Phone);

        }
        [Fact]
        public async void CreateShippingDetails_ShouldReturnShippingDetailsResponse_WhenCreateIsSuccess()
        {
            // Arrange
            Order order = new Order
            {
                Id=1,
                OrderDate = DateTime.Now,
                AccountInfoId =acc1id,



            };

            ShippingDetailsRequest newShippingDetails = new()
            {
                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                Phone = "+228415799",
            };

            int productId = 1;

            ShippingDetails createdShippingDetails = new()
            {
                Id = productId,
                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                Phone = "+228415799",
                OrderId=order.Id,
                Order  = order,

            };

            _mockShippingDetailsRepository
                .Setup(x => x.Create(It.IsAny<ShippingDetails>()))
                .ReturnsAsync(createdShippingDetails);
          

            // Act
            var result = await _shippingDetailsService.InsertShippingsDetails(newShippingDetails);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShippingDetailsResponse>(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal(newShippingDetails.Address, result.Address);
            Assert.Equal(newShippingDetails.City, result.City);
            Assert.Equal(newShippingDetails.PostalCode, result.PostalCode);
            Assert.Equal(newShippingDetails.Country, result.Country);
            
        }
    }

}


