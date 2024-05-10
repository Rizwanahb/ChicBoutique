using H6_ChicBotique.Authorization;
using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Helpers;
using H6_ChicBotique.Repositories;
using H6_ChicBotique.Services;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H6_ChicBotiqueTestProject.ServiceTests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _mockUserRepository = new();
        private readonly Mock<IAccountInfoRepository> _mockAccountInfoRepository = new();
        private readonly Mock<IPasswordEntityRepository> _mockPasswordEntityRepository = new();
        private readonly Mock<IHomeAddressRepository> _mockHomeAddressRepository = new();
        private readonly Mock<IJwtUtils> _mockJwtUtils = new();
        Guid acc1id = Guid.NewGuid();
        Guid acc2id = Guid.NewGuid();

        public UserServiceTests()
        {
            // Initializing the UserService with the mock User repository
            _userService = new UserService(_mockUserRepository.Object,
                _mockPasswordEntityRepository.Object,
                _mockHomeAddressRepository.Object,
                _mockAccountInfoRepository.Object,
                _mockJwtUtils.Object);
        }
        //Test for GetAllAccountInfo method of service
        [Fact]
        public async void GetAllUsers_ShouldReturnListOfUserResponses_WhenAccountInfoExist()
        {
            // Arrange
            // Creating a list of accountInfo
            List<User> users = new();




            users.Add(new User
            {
                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",

                Role = Role.Administrator

            });
            users.Add(new User
            {
                Id = 1,
                FirstName = "test",
                LastName = "test",
                Email = "test@abc.com",

                Role = Role.Member

            });

            // Setting up the mock accountInfo repository to return the list of accountInfo
            _mockUserRepository
                .Setup(x => x.SelectAll())
                .ReturnsAsync(users);

            // Act
            var result = await _userService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.IsType<List<UserResponse>>(result);
        }
        // Test case for GetProductById method when product exists
        [Fact]
        public async void GetUserByEmail_ShouldReturnUserResponse_WhenUserExists()
        {
            // Arrange
            string email = "peter@abc.com";


            User user = new()
            {
                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email =email,
                Role = Role.Administrator
            };


            _mockUserRepository
                .Setup(x => x.SelectByEmail(It.IsAny<string>()))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetIdByEmail(email);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserResponse>(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.Email, result.Email);

        }

        [Fact]
        public async void AuthenticateUser_ShouldReturnUserResponse_WhenLoginIsSuccess()
        {
            // Arrange
           
            LoginRequest loginRequest = new()
            {
                Email="Test@abc.com",
                Password="Test"
            };

            int userId = 1;

           LoginResponse loginResponse = new()
            {
                Id = userId,
                FirstName="Test",
                Email = "Test@abc.com",
                Password = "Test",
                Role = Role.Member,
                Token="tokeniiiiii"
            };
            User user = new()
            {
                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email ="Test@abc.com",
                Role = Role.Administrator
            };
            PasswordEntity pwd = new()
            {
                PasswordId = 1,
                UserId = userId,
                Password = "Test",
                Salt="Test",
                LastUpdated= DateTime.UtcNow,
            };

            _mockUserRepository
                .Setup(x => x.SelectByEmail(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockPasswordEntityRepository
                .Setup(x => x.SelectByUserId(It.IsAny<int>()))
                .ReturnsAsync(pwd);
            // Act
            var result = await _userService.Authenticate(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<LoginResponse>(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(loginResponse.FirstName, result.FirstName);
            Assert.Equal(loginResponse.Email, result.Email);
            
        }
        [Fact]
        public async void CreateMember_ShouldReturnUserResponse_WhenCreateIsSuccess()
        {
            //Arrange
            int userId=1;
            Guid acc1id = Guid.NewGuid();
            UserRegisterRequest newUser= new()
            {
                Email = "peter@abc.com",
                Password = "password",
                FirstName = "Peter",
                LastName = "Aksten",
                Address = "House no:123 , 2700",
                City    =" Ghost street",
                PostalCode= "2700",
                Country="DK",
                Telephone = "12345678",


            };
            HomeAddress homeaddress = new()
            {
                AccountInfoId = acc1id,
                Address = "House no:123 , 2700",
                City    =" Ghost street",
                PostalCode= "2700",
                Country="DK",
               
            };


            User user = new()
            {
                Id=1,
                Email = "peter@abc.com",
               
                FirstName = "Peter",
                LastName = "Aksten",
               Role=Role.Member


            };
            AccountInfo acc = new()
            {
                UserId=1,
            };
            var salt = PasswordHelpers.GenerateSalt();
          

            _mockUserRepository
               .Setup(x => x.Create(It.IsAny<User>()))
               .ReturnsAsync(user);

            _mockAccountInfoRepository
              .Setup(x => x.Create(It.IsAny<AccountInfo>()))
              .ReturnsAsync(acc);

            _mockHomeAddressRepository
        .Setup(x => x.Create(It.IsAny<HomeAddress>()))
        .ReturnsAsync(homeaddress);
            //Act
            var result = await _userService.Register(newUser);

            //Assert         

            Assert.Null(result);
            Assert.IsType<UserResponse>(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(newUser.FirstName, result.FirstName);



        }
        [Fact]
        public async void CreateGuest_ShouldReturnUserResponse_WhenCreateIsSuccess()
        {
            //Arrange
            int userId = 1;
            Guid acc1id = Guid.NewGuid();
            GuestRequest newGuest = new()
            {
                Email = "peter@abc.com",
            
                FirstName = "Peter",
                LastName = "Aksten",
                Address = "House no:123 , 2700",
                City    =" Ghost street",
                PostalCode= "2700",
                Country="DK",
                Telephone = "12345678",


            };
            HomeAddress homeaddress = new()
            {
                AccountInfoId = acc1id,
                Address = "House no:123 , 2700",
                City    =" Ghost street",
                PostalCode= "2700",
                Country="DK",

            };


            User user = new()
            {
                Id=1,
                Email = "peter@abc.com",

                FirstName = "Peter",
                LastName = "Aksten",
                Role=Role.Member


            };
            AccountInfo acc = new()
            {
                UserId=1,
            };

            _mockUserRepository
               .Setup(x => x.Create(It.IsAny<User>()))
               .ReturnsAsync(user);

            _mockAccountInfoRepository
              .Setup(x => x.Create(It.IsAny<AccountInfo>()))
              .ReturnsAsync(acc);

            _mockHomeAddressRepository
        .Setup(x => x.Create(It.IsAny<HomeAddress>()))
        .ReturnsAsync(homeaddress);
            //Act
            var result = await _userService.Register_Guest(newGuest);

            //Assert         

            //Assert.Null(result);
            Assert.IsType<GuestResponse>(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(newGuest.FirstName, result.FirstName);



        }
        [Fact]
        public async void UpdateUser_ShouldReturnUserResponse_WhenUpdateIsSuccess()
        {
            // We do not test if anything actually changed on the DB,
            // we only test that the returned values match the submitted values

            // Arrange
            // Creating a mock AccountInfo for the user
            AccountInfo acc = new()
            {
                UserId=1,
            };

            // Creating a user request with updated values
            UserRequest userRequest = new()
            {
                
                Email = "peter@abc.com",
                Password="test",
                FirstName = "Peter",
                LastName = "Aksten",
                

            };

            // Creating a product with existing values
            int userId = 1;
            User user = new()
            {
                Id=1,
                Email = "peter@abc.com",
                AccountInfo = acc,
                FirstName = "Peter",
                LastName = "Aksten",
                Role=Role.Member


            };

            // Setting up the mock user repository to return the updated user
            _mockUserRepository
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(user);

            // Setting up the mock AccountInfo repository to return the mock AccountInfo
            _mockAccountInfoRepository
                .Setup(x => x.SelectById(It.IsAny<Guid>()))
                .ReturnsAsync(acc);
            
            // Act
            var result = await _userService.Update(userId, userRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserResponse>(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(userRequest.FirstName, result.FirstName);
            Assert.Equal(userRequest.LastName, result.LastName);
            Assert.Equal(userRequest.Email, result.Email);
          
        }
    }
}