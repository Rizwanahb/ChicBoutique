using H6_ChicBotique.Database;
using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.Repositories;
using Microsoft.EntityFrameworkCore;


namespace H6_ChicBotiqueTestProject.RepositoryTests
{
    // Test class for CategoryRepository
    public class CategoryRepositoryTests
    {
        // Private fields to store testing environment and objects
        private readonly DbContextOptions<ChicBotiqueDatabaseContext> _options;
        private readonly ChicBotiqueDatabaseContext _context;
        private readonly CategoryRepository _categoryRepository;

        // Constructor to set up the testing environment
        public CategoryRepositoryTests()
        {
            // Setting up in-memory database options
            _options = new DbContextOptionsBuilder<ChicBotiqueDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ChicBotique")
                .Options;

            // Creating an instance of the context and repository
            _context = new(_options);
            _categoryRepository = new(_context);
        }

        // Test case for SelectAllCategories method
        [Fact]
        public async void SelectAllCategories_ShouldReturnListOfCategories_WhenCategoryExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Adding categories to the in-memory database
            _context.Category.Add(new Category
            {
                Id = 1,
                CategoryName = "Kids"
            });
            _context.Category.Add(new Category
            {
                Id = 2,
                CategoryName = "Men"
            });
            _context.Category.Add(new Category
            {
                Id = 3,
                CategoryName = "Women"
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _categoryRepository.SelectAllCategories();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
            Assert.Equal(3, result.Count);
        }

        // Test case for SelectAllCategories method
        [Fact]
        public async void SelectAllCategories_ShouldReturnEmptyListOfCategories_WhenNoCategoryExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _categoryRepository.SelectAllCategories();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
            Assert.Empty(result);
        }

        // Test case for SelectCategoryById method
        [Fact]
        public async void SelectCategoryById_ShouldReturnCategory_WhenCategoryExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();
            int categoryId = 1;

            // Adding a category to the in-memory database
            _context.Category.Add(new Category
            {
                Id = 1,
                CategoryName = "Kids"
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _categoryRepository.SelectCategoryById(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(categoryId, result.Id);
        }

        // Test case for SelectCategoryById method
        [Fact]
        public async void SelectCategoryById_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _categoryRepository.SelectCategoryById(1);

            // Assert
            Assert.Null(result);
        }

        // Test case for InsertNewCategory method
        [Fact]
        public async void InsertNewCategory_ShouldFailToAddNewCategory_WhenCategoryIdAlreadyExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Creating a category with an existing Id
            Category category = new()
            {
                Id = 1,
                CategoryName = "Kids"
            };

            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            // Act
            async Task action() => await _categoryRepository.InsertNewCategory(category);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        // Test case for UpdateExistingCategory method
        [Fact]
        public async void UpdateExistingCategory_ShouldChangeValuesOnCategory_WhenCategoryExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();
            int categoryId = 1;

            // Creating a new category and updating it
            Category newCategory = new()
            {
                Id = categoryId,
                CategoryName = "Kids"
            };

            _context.Category.Add(newCategory);
            await _context.SaveChangesAsync();

            Category updateCategory = new()
            {
                Id = categoryId,
                CategoryName = "updated Kids"
            };

            // Act
            var result = await _categoryRepository.UpdateExistingCategory(categoryId, updateCategory);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(categoryId, result.Id);
            Assert.Equal(updateCategory.CategoryName, result.CategoryName);
        }

        // Test case for UpdateExistingCategory method
        [Fact]
        public async void UpdateExistingCategory_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();
            int categoryId = 1;

            // Creating an update for a non-existing category
            Category updateCategory = new()
            {
                Id = categoryId,
                CategoryName = "Kids"
            };

            // Act
            var result = await _categoryRepository.UpdateExistingCategory(categoryId, updateCategory);

            // Assert
            Assert.Null(result);
        }

        // Test case for DeleteCategoryById method
        [Fact]
        public async void DeleteCategoryById_ShouldReturnDeleteCategory_WhenCategoryIsDeleted()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();
            int categoryId = 1;

            // Creating a new category and deleting it
            Category newCategory = new()
            {
                Id = categoryId,
                CategoryName = "Kids"
            };

            _context.Category.Add(newCategory);
            await _context.SaveChangesAsync();

            // Act
            var result = await _categoryRepository.DeleteCategoryById(categoryId);
            var category = await _categoryRepository.SelectCategoryById(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(categoryId, result.Id);
            Assert.Null(category);
        }

        // Test case for DeleteCategoryById method
        [Fact]
        public async void DeleteCategoryById_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Creating a non-existing category
            _context.Add(new Category { Id = 1, CategoryName = "Kids" });

            // Act
            var result = await _categoryRepository.DeleteCategoryById(1);

            // Assert
            Assert.Null(result);
        }
    }
}


