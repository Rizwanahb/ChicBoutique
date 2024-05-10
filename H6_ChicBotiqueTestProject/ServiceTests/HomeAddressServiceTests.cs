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
    public class HomeAddressServiceTests
    {
        private readonly HomeAddressService _homeAddressService;
        private readonly Mock<IHomeAddressRepository> _mockHomeAddressRepository = new();
       

        Guid acc1id = Guid.NewGuid();
        Guid acc2id = Guid.NewGuid();

        public HomeAddressServiceTests()
        {
            // Initializing the AccountInfoService with the mock accountInfo repository
            _homeAddressService = new HomeAddressService(_mockHomeAddressRepository.Object);
        }
        //Test for GetAllHomeAddress method of service
        [Fact]
        public async void GetAllHomeAddress_ShouldReturnListOfHomeAddressResponses_WhenHomeAddressExist()
        {
            // Arrange
            // Creating a list of HomeAddress
            List<HomeAddress> homeAddresses = new List<HomeAddress>(); ;


          
            AccountInfo accountInfo = new AccountInfo
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,
                UserId=1,
                

            };

            homeAddresses.Add(new HomeAddress
            {
                AccountInfoId = acc1id,
                Id = 1,

                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                TelePhone = "+228415799",
                AccountInfo=accountInfo

            });


            // Setting up the mock HomeAddress repository to return the list of HomeAddress
            _mockHomeAddressRepository
                .Setup(x => x.SelectAll())
                .ReturnsAsync(homeAddresses);

            // Act
            var result = await _homeAddressService.GetAllHomeAddresses();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.IsType<List<HomeAddressResponse>>(result);
        }
        // Test case for GetHomeAddressById method when HomeAddress exists
        [Fact]
        public async void GetHomeAddressById_ShouldReturnHomeAddressResponse_WhenHomeAddressExists()
        {
            // Arrange
            int homeAddressId = 1;
            AccountInfo accountInfo = new AccountInfo
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,
                UserId=1,


            };

            HomeAddress homeAddress = new HomeAddress
            {
                AccountInfoId = acc1id,
                Id = homeAddressId,

                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                TelePhone = "+228415799",
                AccountInfo=accountInfo

            };
            _mockHomeAddressRepository
                .Setup(x => x.SelectById(It.IsAny<int>()))
                .ReturnsAsync(homeAddress);

            // Act
            var result = await _homeAddressService.GetHomeAddressById(homeAddressId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<HomeAddressResponse>(result);
            Assert.Equal(homeAddress.Id, result.Id);
            Assert.Equal(homeAddress.Address, result.Address);
            Assert.Equal(homeAddress.City, result.City);
            Assert.Equal(homeAddress.PostalCode, result.PostalCode);
            Assert.Equal(homeAddress.Country, result.Country);
            Assert.Equal(homeAddress.TelePhone, result.Phone);

        }
        [Fact]
        public async void UpdateHomeAddress_ShouldReturnHomeAddressResponse_WhenUpdateIsSuccess()
        {
            // We do not test if anything actually changed on the DB,
            // we only test that the returned values match the submitted values

            // Arrange
            // Creating a mock category for the product
            int homeAddressId = 1;
            AccountInfo accountInfo = new AccountInfo
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,
                UserId=1,


            };


            // Creating a homeAddress request with updated values
            HomeAddressRequest homeAddressRequest = new()
            {
                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                Phone = "+228415799",
                //AccountInfo=accountInfo
           

            };
          HomeAddress newHomeAddress= MapHomeAddressRequestToHomeAddress(homeAddressRequest);
            
            // Creating a homeAddress with existing values

            HomeAddress homeAddress = new HomeAddress
            {
            
                Id = homeAddressId,

                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                TelePhone = "+228415799",
                AccountInfoId = accountInfo.Id,
                AccountInfo = accountInfo

            };
            // Setting up the mock category repository to return the mock category
            _mockHomeAddressRepository
                .Setup(x => x.SelectById(It.IsAny<int>()))
                .ReturnsAsync(newHomeAddress);

            // Setting up the mock HomeAddress repository to return the updated product
            _mockHomeAddressRepository
                .Setup(x => x.Update(It.IsAny<HomeAddress>()))
                .ReturnsAsync(homeAddress);

            homeAddress.Id = homeAddressId; //saving HomeAddressId to requested HomeAddress.Id
            homeAddress.AccountInfoId = newHomeAddress.AccountInfoId;

            // Act
            var result = await _homeAddressService.UpdateHomeAddress(homeAddressId, homeAddressRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<HomeAddressResponse>(result);
            Assert.Equal(homeAddressId, result.Id);
            Assert.Equal(homeAddressRequest.Address, result.Address);
            Assert.Equal(homeAddressRequest.City, result.City);
            Assert.Equal(homeAddressRequest.Country, result.Country);
            Assert.Equal(homeAddressRequest.PostalCode, result.PostalCode);
            Assert.Equal(homeAddressRequest.Phone, result.Phone);
        }

        private static HomeAddress MapHomeAddressRequestToHomeAddress(HomeAddressRequest HomeAddressRequest)
        {
            return new HomeAddress()
            {


                Address = HomeAddressRequest.Address,
                City = HomeAddressRequest.City,
                PostalCode = HomeAddressRequest.PostalCode,
                Country=HomeAddressRequest.Country,
                TelePhone=HomeAddressRequest.Phone
            };
        }
    }

}


