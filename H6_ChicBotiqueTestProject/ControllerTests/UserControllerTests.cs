using H6_ChicBotique.Controllers;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Helpers;
using H6_ChicBotique.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace H6_ChicBotiqueTestProject.ControllerTests
{
    public class UserControllerTest
    {
        private readonly UserController _userController;
        private readonly Mock<IUserService> _mockuserService = new();
        public UserControllerTest()
        {
            _userController = new(_mockuserService.Object);
        }

        [Fact]
        public async void GetAllUsers_ShouldReturnStatusCode200_WhenuserExists()
        {
            //Arrange
            List<UserResponse> users = new();
            users.Add(new()
            {
                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",

                Role = Role.Administrator

            });

            users.Add(new()
            {
                Id = 2,
                FirstName = "Rizwanah",
                LastName = "Mustafa",
                Email = "riz@abc.com",
                Role = Role.Member
            });

            _mockuserService.Setup(x => x.GetAll()).ReturnsAsync(users);

            //Act
            var result = await _userController.GetAll();

            //Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }
        [Fact]
        public async void GetAllUsers_ShouldReturnStatusCode204_WhenNouserExists()
        {
            //Arrange

            List<UserResponse> users = new();

            _mockuserService
                .Setup(x => x.GetAll())
                .ReturnsAsync(users);

            //Act
            var result = await _userController.GetAll();

            //Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(204, statusCodeResult.StatusCode);
        }
        [Fact]
        public async void GetAllUsers_ShouldReturnStatusCode500_WhenNullIsReturnedFromService()
        {
            //Arrange                      
            _mockuserService
                .Setup(x => x.GetAll())
                .ReturnsAsync(() => null);

            //Act
            var result = await _userController.GetAll();

            //Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange                      
            _mockuserService
                .Setup(x => x.GetAll())
                .ReturnsAsync(() => throw new System.Exception("This is an exception"));

            //Act
            var result = await _userController.GetAll();

            //Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        [Fact]
        public async void GetUserByEmail_ShouldReturnStatusCode200_WhenDataExists()
        {
            //Arrange
            // int AccountId = 1;
            string Email = "peter@abc.com";
            UserResponse User = new()
            {
                Id = 1,
                FirstName = "Peter",

                LastName = "Aksten",
                Email = "peter@abc.com",

                Role = Role.Administrator

            };

            _mockuserService
                .Setup(x => x.GetIdByEmail(It.IsAny<string>()))
                .ReturnsAsync(User);

            //Act
            var result = await _userController.GetUserByEmail(Email);

            //Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }
        [Fact]
        public async void CreateMember_ShouldReturnStatusCode200_WhenUserIsSuccessfullyCreated()
        {
            // Arrange
            UserRegisterRequest newUser = new UserRegisterRequest
            {
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",    
                Password = "password",
                Address="husum",
                PostalCode = "12345",
                City    ="cph",
                Country="dk",
                Telephone="+451234567"

           

            };

            // Setup the mock ProductService to return a product response
            UserResponse userResponse = new UserResponse
            {
                Id=1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",

                Role = Role.Member


            };
            _mockuserService.Setup(x => x.Register(It.IsAny<UserRegisterRequest>())).ReturnsAsync(userResponse);

            // Act
            var result = await _userController.Register(newUser);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void CreateGuest_ShouldReturnStatusCode200_WhenGuestUserIsSuccessfullyCreated()
        {
            // Arrange
            GuestRequest newGuest = new GuestRequest
            {
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",
               
                Address="husum",
                PostalCode = "12345",
                City    ="cph",
                Country="dk",
                Telephone="+451234567"



            };

            // Setup the mock UserService to return a GuestResponse
            GuestResponse guestResponse = new GuestResponse
            {
                Id=1,
                FirstName = "Sara",
                LastName = "Ahmed",
                Email = "sara@abc.com",

                Role = Role.Guest


            };
            _mockuserService.Setup(x => x.Register_Guest(It.IsAny<GuestRequest>())).ReturnsAsync(guestResponse);

            // Act
            var result = await _userController.guestRegister(newGuest);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }
        [Fact]
        public async void Login_ShouldReturnStatusCode200_WhenUserIsSuccessfullylogged()
        {
            // Arrange
            LoginRequest checkMember = new LoginRequest
            {
               
                Email = "peter@abc.com",
                Password="12345",
            };

            // Setup the mock UserService to return a GuestResponse
            LoginResponse loginResponse = new LoginResponse
            {
                Id=1,
                FirstName = "Sara",
                Password    = "12345",
                Email = "sara@abc.com",
                Token = "12345",
                Role = Role.Member


            };
            _mockuserService.Setup(x => x.Authenticate(It.IsAny<LoginRequest>())).ReturnsAsync(loginResponse);

            // Act
            var result = await _userController.Authenticate(checkMember);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Update_ShouldReturnStatusCode200_WhenUserIsSuccessfullyUpdated()
        {
            // Arrange
            UserRequest updateUser = new UserRequest
            {
                FirstName = "Test",
                LastName    = "Test",
                Email = "Test@abc.com",
                Password    = "Test",
            };

            int userId = 1;

            // Create a sample user response for the mock UserService
            UserResponse userResponse = new UserResponse
            {
                Id = 1,
                FirstName= "Test",
                LastName= "Test",
                Email = "Test@abc.com",
                
            };

            // Setup the mock ProductService to return the sample product response
            _mockuserService.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UserRequest>()))
                .ReturnsAsync(userResponse);

            // Act
            var result = await _userController.Update(userId, updateUser);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }
        //Testing for ChangePassword Method of Controller
        [Fact]
        public async void ChangePassword_ShouldReturnStatusCode200_WhenUserIsSuccessfullychangedPassword()
        {
            // Arrange
            PasswordEntityRequest updateUserPassword = new PasswordEntityRequest
            {
                UserId = 1,
                Password    = "Test",
            };

            int userId = 1;

            bool response = true;
           

            // Setup the mock ProductService to return the sample product response
            _mockuserService.Setup(x => x.UpdatePassword( It.IsAny<PasswordEntityRequest>()))
                .ReturnsAsync(response);

            // Act
            var result = await _userController.ChangePassword(updateUserPassword);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

    }
}
