using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.Database;
using H6_ChicBotique.Repositories;
using Microsoft.EntityFrameworkCore;
using H6_ChicBotique.Helpers;

namespace H6_ChicBotiqueTestProject.RepositoryTests
{
    public class AccountInfoRepositoryTests
    {

        // Private fields to store testing environment and objects
        private readonly DbContextOptions<ChicBotiqueDatabaseContext> _options;
        private readonly ChicBotiqueDatabaseContext _context;
        private readonly AccountInfoRepository _accountInfoRepository;
        Guid acc1id = Guid.NewGuid();
        Guid acc2id = Guid.NewGuid();

        // Constructor to set up the testing environment

        public AccountInfoRepositoryTests()
        {
            // Setting up in-memory database options
            _options = new DbContextOptionsBuilder<ChicBotiqueDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ChicBotique")
                .Options;

            // Creating an instance of the context and repository
            _context = new(_options);
            _accountInfoRepository = new(_context);
        }

        // Test case for SelectAllCategories method
        [Fact]
        public async void SelectAllAccountInfo_ShouldReturnListOfAccountInfo_WhenAccountInfoExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Adding categories to the in-memory database
            _context.AccountInfo.Add(new AccountInfo
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,
                UserId=1,


            });

            _context.AccountInfo.Add(new AccountInfo
            {
                Id= acc2id,
                CreatedDate = DateTime.UtcNow,
                UserId = 2
            });
            _context.User.Add(new User
            {
                Id = 1,
                FirstName = "Peter",
                LastName = "Aksten",
                Email = "peter@abc.com",
                Role = Role.Administrator


            });
            _context.User.Add(new User
            {
                Id = 2,
                FirstName = "Rizwanah",
                LastName = "Mustafa",
                Email = "riz@abc.com",
                Role = Role.Member
            });
            _context.HomeAddress.Add(new HomeAddress
            {
                AccountInfoId = acc1id,
                Id = 1,

                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                TelePhone = "+228415799"

            });
            _context.HomeAddress.Add(new HomeAddress
            {
                AccountInfoId = acc2id,
                Id = 2,
                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                TelePhone = "+228415799"
            });
            _context.Order.Add(new Order
            {
                Id = 1,
                OrderDate= DateTime.Now,


            });
            _context.Order.Add(new Order
            {
                Id = 2,
                OrderDate= DateTime.Now,


            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _accountInfoRepository.SelectAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<AccountInfo>>(result);
            Assert.Equal(2, result.Count);
        }

        // Test case for SelectAllCategories method


        [Fact]
        public async void SelectAllAccountInfo_ShouldReturnEmptyListOfAccountInfo_WhenNoAccountInfoExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _accountInfoRepository.SelectAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<AccountInfo>>(result);
            Assert.Empty(result);
        }
        // Test case for SelectCategoryById method
        [Fact]
        public async void SelectAccountInfoById_ShouldReturnAccountInfo_WhenAccountInfoExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();
            Guid accountInfoId = acc1id;

            // Adding a category to the in-memory database
            _context.AccountInfo.Add(new AccountInfo
            {
                Id = accountInfoId,
                CreatedDate = DateTime.UtcNow,
                UserId=1,
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _accountInfoRepository.SelectById(accountInfoId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AccountInfo>(result);
            Assert.Equal(accountInfoId, result.Id);
        }

        // Test case for SelectCategoryById method
        [Fact]
        public async void SelectAccountInfoById_ShouldReturnNull_WhenAccountInfoDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _accountInfoRepository.SelectById(acc1id);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async void InsertNewAccountInfo_ShouldFailToAddNewAccountInfo_WhenAccountInfoIdAlreadyExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Creating a AccountInfo with an existing Id
            AccountInfo accountInfo = new()
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,
                UserId=1,


            };

            _context.AccountInfo.Add(accountInfo);
            await _context.SaveChangesAsync();

            // Act
            async Task action() => await _accountInfoRepository.Create(accountInfo);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        // Test case for UpdateExistingAccountInfo method
        [Fact]
        public async void UpdateExistingAccountInfo_ShouldChangeValuesOnAccountInfo()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();
            Guid accountInfoId = acc1id;

            // Creating a new category and updating it
            AccountInfo newAccountInfo = new()
            {
                Id = accountInfoId,
                CreatedDate = DateTime.UtcNow,
                UserId=1,


            };

            _context.AccountInfo.Add(newAccountInfo);
            await _context.SaveChangesAsync();

            AccountInfo updateAccountInfo = new()
            {
                Id = accountInfoId,
                CreatedDate = DateTime.UtcNow,
                UserId=2
            };

            // Act
            var result = await _accountInfoRepository.Update( updateAccountInfo);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AccountInfo>(result);
            Assert.Equal(accountInfoId, result.Id);
            Assert.Equal(updateAccountInfo.UserId, result.UserId);
        }
        // Test case for DeleteAccountInfoById method
        [Fact]
        public async void DeleteAccountInfoById_ShouldReturnDeleteAccountInfo_WhenAccountInfoIsDeleted()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();
            Guid accountInfoId = acc1id;

            // Creating a new category and deleting it
            AccountInfo newAccountInfo = new()
            {
                Id = accountInfoId,
                CreatedDate = DateTime.UtcNow,
                UserId=1,

            };

            _context.AccountInfo.Add(newAccountInfo);
            await _context.SaveChangesAsync();

            // Act
            var result = await _accountInfoRepository.Delete(accountInfoId);
            var accountInfo = await _accountInfoRepository.SelectById(accountInfoId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AccountInfo>(result);
            Assert.Equal(accountInfoId, result.Id);
            Assert.Null(accountInfo);
        }

    }
}
