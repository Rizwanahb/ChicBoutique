
using H6_ChicBotique.Database;
using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.Helpers;
using H6_ChicBotique.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H6_ChicBotiqueTestProject.RepositoryTests
{
    public class UserRepositoryTest
    {
        private readonly DbContextOptions<ChicBotiqueDatabaseContext> _options;
        private readonly ChicBotiqueDatabaseContext _context;
        private readonly UserRepository _userRepository;
        private readonly Mock<H6_ChicBotique.Authorization.IJwtUtils> jwt = new();

        public UserRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<ChicBotiqueDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ChicBotique")
                .Options;

            _context = new(_options);
            _userRepository = new(_context);
        }

        [Fact]
        public async void SelectAllUsers_ShouldReturnListOfUsers_WhenUsersExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.User.Add(new()
            {
                Id = 1,
                FirstName = "Peter",

                LastName = "Aksten",
                Email = "peter@abc.com",

                Role = Role.Administrator

            });

            _context.User.Add(new()

            {
                Id = 2,
                FirstName = "Rizwanah",

                LastName = "Mustafa",

                Email = "riz@abc.com",

                Role = Role.Member



            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _userRepository.SelectAll();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<User>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void SelectAllUsers_ShouldReturnEmptyListOfUsers_WhenNoUsersExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _userRepository.SelectAll();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<User>>(result);
            Assert.Empty(result);
        }
        [Fact]
        public async void SelectUserById_ShouldReturnUser_WhenUserExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int userId = 1;


            _context.User.Add(new()
            {
                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",

                Role = Role.Administrator
            });


            await _context.SaveChangesAsync();

            //Act

            var result = await _userRepository.SelectById(userId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async void SelectUserById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _userRepository.SelectById(1);

            //Assert
            Assert.Null(result);

        }
        [Fact]
        public async void InsertNewUser_ShouldAddNewIdToUser_WhenSavingToDatabase()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedNewId = 1;


            User newUser = new()
            {
                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",

                Role = Role.Administrator

            };



            //Act

            var result = await _userRepository.Create(newUser);


            //Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(expectedNewId, result.Id);
        }
        [Fact]
        public async void InsertNewUser_ShouldFailToAddNewUser_WhenUserIdAlreadyExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedNewId = 1;

            User user = new()
            {
                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",

                Role = Role.Administrator
            };

            await _context.SaveChangesAsync();

            //Act
            var result = await _userRepository.Create(user);


            //Assert       
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(expectedNewId, result.Id);

        }
        [Fact]
        public async void UpdateExistingUser_ShouldChangeValuesOnUser_WhenUserExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int userId = 1;

            User newUser = new()
            {

                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",

                Role = Role.Administrator
            };

            _context.User.Add(newUser);
            await _context.SaveChangesAsync();

            User updateUser = new()
            {

                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",

                Role = Role.Administrator
            };


            //Act
            var result = await _userRepository.Update(userId, updateUser);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(updateUser.FirstName, result.FirstName);
            Assert.Equal(updateUser.LastName, result.LastName);
            Assert.Equal(updateUser.Email, result.Email);
            //Assert.Equal(updateUser.Address, result.Address);
            // Assert.Equal(updateUser.Telephone, result.Telephone);   

        }
        [Fact]
        public async void UpdateExistingUser_ShouldReturnNull_WhenUserDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int userId = 1;

            User updateUser = new()
            {

                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",

                Role = Role.Administrator
            };


            //Act
            var result = await _userRepository.Update(userId, updateUser);

            //Asert
            Assert.Null(result);

        }
        [Fact]
        public async void DeleteUserById_ShouldReturnDeletedUser_WhenUserIsDeleted()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int userId = 1;

            User newUser = new()
            {

                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",

                Role = Role.Administrator
            };

            _context.User.Add(newUser);
            await _context.SaveChangesAsync();


            //Act
            var result = await _userRepository.Delete(userId);
            var user = await _userRepository.SelectById(userId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(userId, result.Id);
            Assert.Null(user);
        }
        [Fact]
        public async void DeleteUserById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _userRepository.Delete(1);


            //Assert
            Assert.Null(result);

        }

    }
}
