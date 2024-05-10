using H6_ChicBotique.Database;
using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.Helpers;
using H6_ChicBotique.Repositories;
using Microsoft.EntityFrameworkCore;

namespace H6_ChicBotiqueTestProject.RepositoryTests
{
    public class PasswordEntityRepositoryTests
    {
        private readonly DbContextOptions<ChicBotiqueDatabaseContext> _options;
        private readonly ChicBotiqueDatabaseContext _context;
        private readonly PasswordEntityRepository _passwordEntityRepository;
        public string salt = PasswordHelpers.GenerateSalt();
        public PasswordEntityRepositoryTests()
        {
            // Setting up in-memory database options
            _options = new DbContextOptionsBuilder<ChicBotiqueDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ChicBotique")
                .Options;

            // Creating an instance of the context and repository
            _context = new(_options);
            _passwordEntityRepository = new(_context);
        }
        // Test case for SelectAllPasswordEntity method
        [Fact]
        public async void SelectAllPasswordEntity_ShouldReturnListOfPasswordEntity_WhenPasswordEntityExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Adding PasswordEntity to the in-memory database
            var salt = PasswordHelpers.GenerateSalt();
            _context.PasswordEntity.Add(new PasswordEntity
            {
                PasswordId = 1,
                UserId = 1,
                Password = PasswordHelpers.HashPassword("password" + salt),
                Salt = salt,

            });
            _context.PasswordEntity.Add(new PasswordEntity
            {
                PasswordId = 2,
                UserId = 2,
                Password = PasswordHelpers.HashPassword("password1" + salt),
                Salt = salt,
            });

        

          

            await _context.SaveChangesAsync();

            // Act
            var result = await _passwordEntityRepository.SelectAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<PasswordEntity>>(result);
            Assert.Equal(2, result.Count);
        }
        // Test case for SelectPasswordEntityById method
        [Fact]
        public async void SelectPasswordEntityById_ShouldReturnPasswordEntity_WhenHomeAddressExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int passwordEntityId = 1;
            // Adding a PasswordEntity to the in-memory database
            _context.PasswordEntity.Add(new PasswordEntity
            {
                PasswordId = 1,
                UserId = 1,
                Password = PasswordHelpers.HashPassword("password" + salt),
                Salt = salt
            });
         


            await _context.SaveChangesAsync();

            // Act
            var result = await _passwordEntityRepository.SelectByUserId(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PasswordEntity>(result);
            Assert.Equal(passwordEntityId, result.PasswordId);
        }

        [Fact]
        public async void InsertNewPasswordEntity_ShouldFailToAddPasswordEntity_WhenPasswordEntityIdAlreadyExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Creating a PasswordEntity with an existing Id
            PasswordEntity passwordEntity = new PasswordEntity
            {
                PasswordId = 1,
                UserId = 1,
                Password = PasswordHelpers.HashPassword("password" + salt),
                Salt = salt
            };

            _context.PasswordEntity.Add(passwordEntity);
            await _context.SaveChangesAsync();

            // Act
            async Task action() => await _passwordEntityRepository.CreatePassword(passwordEntity);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }
        // Test case for UpdateExistingPasswordEntity method
        [Fact]
        public async void UpdateExistingPasswordEntity_ShouldChangeValuesOnPasswordEntity()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();
            int passwordEntityId = 1;

            // Creating a new PasswordEntity and updating it
            PasswordEntity newPasswordEntity = new PasswordEntity
            {
                PasswordId = 1,
                UserId = 1,
                Password = PasswordHelpers.HashPassword("password" + salt),
                Salt = salt


            };
        

            _context.PasswordEntity.Add(newPasswordEntity);
            await _context.SaveChangesAsync();

            PasswordEntity updatePasswordEntity = new PasswordEntity
            {
                PasswordId = 1,
                UserId = 1,
                Password = PasswordHelpers.HashPassword("pass" + salt),
                Salt = salt
            };
           

            // Act
            var result = await _passwordEntityRepository.UpdatePassword(updatePasswordEntity);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PasswordEntity>(result);
            Assert.Equal(passwordEntityId, result.PasswordId);
            Assert.Equal(updatePasswordEntity.Password, result.Password);
        }


    }
}
