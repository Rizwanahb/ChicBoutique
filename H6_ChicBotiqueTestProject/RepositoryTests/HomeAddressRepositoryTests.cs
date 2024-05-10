using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.Database;
using H6_ChicBotique.Repositories;
using Microsoft.EntityFrameworkCore;
using H6_ChicBotique.Helpers;

namespace H6_ChicBotiqueTestProject.RepositoryTests
{
    public class HomeAddressRepositoryTests
    {

        // Private fields to store testing environment and objects
        private readonly DbContextOptions<ChicBotiqueDatabaseContext> _options;
        private readonly ChicBotiqueDatabaseContext _context;
        private readonly HomeAddressRepository _homeAddressRepository;
        Guid acc1id = Guid.NewGuid();
        Guid acc2id = Guid.NewGuid();

        // Constructor to set up the testing environment

        public HomeAddressRepositoryTests()
        {
            // Setting up in-memory database options
            _options = new DbContextOptionsBuilder<ChicBotiqueDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ChicBotique")
                .Options;

            // Creating an instance of the context and repository
            _context = new(_options);
            _homeAddressRepository = new(_context);
        }

        // Test case for SelectAllHomeAddress method
        [Fact]
        public async void SelectAllHomeAddress_ShouldReturnListOfHomeAddress_WhenHomeAddressExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Adding HomeAddress to the in-memory database

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

            _context.AccountInfo.Add(new AccountInfo
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,
                UserId=1,


            });

            _context.AccountInfo.Add(new AccountInfo
            {
                Id= acc2id,
                UserId = 2
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _homeAddressRepository.SelectAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<HomeAddress>>(result);
            Assert.Equal(2, result.Count);
        }

        // Test case for SelectAllhomeAddress method


        [Fact]
        public async void SelectAllHomeAddress_ShouldReturnEmptyListOfHomeAddress_WhenNoHomeAddressExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _homeAddressRepository.SelectAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<HomeAddress>>(result);
            Assert.Empty(result);
        }
        // Test case for SelectHomeAddressById method
        [Fact]
        public async void SelectHomeAddressById_ShouldReturnHomeAddress_WhenHomeAddressExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int homeAddressId = 1;
            // Adding a HomeAddress to the in-memory database
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
            _context.AccountInfo.Add(new AccountInfo
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,
                UserId=1,


            });

           
            await _context.SaveChangesAsync();

            // Act
            var result = await _homeAddressRepository.SelectById(homeAddressId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<HomeAddress>(result);
            Assert.Equal(homeAddressId, result.Id);
        }

        // Test case for SelectHomeAddressById method
        [Fact]
        public async void SelectHomeAddressById_ShouldReturnNull_WhenHomeAddressDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _homeAddressRepository.SelectById(1);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async void InsertNewHomeAddress_ShouldFailToAddNewHomeAddress_WheHomeAddressIdAlreadyExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Creating a HomeAddress with an existing Id
            HomeAddress homeAddress = new HomeAddress
            {
                AccountInfoId = acc1id,
                Id = 1,

                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                TelePhone = "+228415799"
            };

            _context.HomeAddress.Add(homeAddress);
            await _context.SaveChangesAsync();

            // Act
            async Task action() => await _homeAddressRepository.Create(homeAddress);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        // Test case for UpdateExistinghomeAddress method
        [Fact]
        public async void UpdateExistingHomeAddress_ShouldChangeValuesOnHomeAddress()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();
            int homeAddressId = 1;
            
            // Creating a new homeAddress and updating it
            HomeAddress newHomeAddress = new HomeAddress
            {
                AccountInfoId = acc1id,
                Id=homeAddressId,
                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                TelePhone = "+228415799"


            };
           /*AccountInfo newaccountInfo=new AccountInfo
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,
                UserId=1,


            };*/


            _context.HomeAddress.Add(newHomeAddress);
            await _context.SaveChangesAsync();

            HomeAddress updateHomeAddress = new HomeAddress
            {
                AccountInfoId = acc1id,
                Id=homeAddressId,
                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2600",
                Country = "Danmark",
                TelePhone = "+228415799"
            };
            //AccountInfo updateaccountInfo = new AccountInfo
            //{
            //    Id = acc1id,
            //    CreatedDate = DateTime.UtcNow,
            //    UserId=1,


            //};

            // Act
            var result = await _homeAddressRepository.Update(updateHomeAddress);

            // Assert
           Assert.NotNull(result);
            Assert.IsType<HomeAddress>(result);
            Assert.Equal(homeAddressId, result.Id);
            Assert.Equal(updateHomeAddress.PostalCode, result.PostalCode);
        }
       
    }
}
