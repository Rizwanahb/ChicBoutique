using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Repositories;
using H6_ChicBotique.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H6_ChicBotiqueTestProject.ServiceTests
{
    public class CategoryServiceTests
    {
        private readonly CategoryService _categoryService;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository = 
            new Mock<ICategoryRepository>();

        public CategoryServiceTests()
        {
            // Initializing the CategoryService with the mock category repository
            _categoryService = new CategoryService(_mockCategoryRepository.Object);
        }

        [Fact]
        public async void GetAllCategories_ShouldReturnListOfCategoryResponses_WhenCategoriesExist()
        {
            // Arrange
            // Creating a list of categories
            List<Category> categories = new List<Category>();

            categories.Add(new Category
            {
                Id = 1,
                CategoryName = "Kids"
            });

            categories.Add(new Category
            {
                Id = 2,
                CategoryName = "Men"
            });

            categories.Add(new Category
            {
                Id = 3,
                CategoryName = "women"
            });

            // Setting up the mock category repository to return the list of categories
            _mockCategoryRepository
                .Setup(x => x.SelectAllCategories())
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.IsType<List<CategoryResponse>>(result);
        }

        [Fact]
        public async void GetAllCategories_ShouldReturnEmptyListOfCategoryResponses_WhenNoCategoriesExist()
        {
            // Arrange
            // Creating an empty list of categories
            List<Category> categories = new List<Category>();

            // Setting up the mock category repository to return an empty list of categories
            _mockCategoryRepository
                .Setup(x => x.SelectAllCategories())
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsType<List<CategoryResponse>>(result);
        }

        [Fact]
        public async void GetCategoryById_ShouldReturnCategoryResponse_WhenCategoryExists()
        {
            // Arrange
            // Setting up the category ID for testing
            int categoryId = 1;

            // Creating a mock category with existing values
            Category category = new Category
            {
                Id = categoryId,
                CategoryName = "Kids"
            };

            // Setting up the mock category repository to return the mock category
            _mockCategoryRepository
                .Setup(x => x.SelectCategoryById(It.IsAny<int>()))
                .ReturnsAsync(category);

            // Act
            var result = await _categoryService.GetCategoryById(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CategoryResponse>(result);
            Assert.Equal(category.Id, result.Id);
            Assert.Equal(category.CategoryName, result.CategoryName);
        }

        [Fact]
        public async void GetCategoryById_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            // Setting up the category ID for testing
            int categoryId = 1;

            // Setting up the mock category repository to return null
            _mockCategoryRepository
                .Setup(x => x.SelectCategoryById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _categoryService.GetCategoryById(categoryId);

            // Assert
            Assert.Null(result);
        }

         [Fact]
         public async void CreateCategory_ShouldReturnCategoryResponse_WhenCreateIsSuccess()
            {
                // Arrange
                // Creating a new category request
                CategoryRequest newCategory = new CategoryRequest
                {
                    CategoryName = "Kids"
                };

                // Setting up the category ID for testing
                int categoryId = 1;

                // Creating a created category with existing values
                Category createdCategory = new Category
                {
                    Id = categoryId,
                    CategoryName = "Kids"
                };

                // Setting up the mock category repository to return the created category
                _mockCategoryRepository
                    .Setup(x => x.InsertNewCategory(It.IsAny<Category>()))
                    .ReturnsAsync(createdCategory);

                // Act
                var result = await _categoryService.CreateCategory(newCategory);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<CategoryResponse>(result);
                Assert.Equal(categoryId, result.Id);
                Assert.Equal(newCategory.CategoryName, result.CategoryName);
            }

            [Fact]
            public async void CreateCategory_ShouldReturnNull_WhenRepositoryReturnsNull()
            {
                // Arrange
                // Creating a new category request
                CategoryRequest newCategory = new CategoryRequest
                {
                    CategoryName = "Kids"
                };

                // Setting up the mock category repository to return null
                _mockCategoryRepository
                    .Setup(x => x.InsertNewCategory(It.IsAny<Category>()))
                    .ReturnsAsync(() => null);

                // Act
                var result = await _categoryService.CreateCategory(newCategory);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async void UpdateCategory_ShouldReturnCategoryResponse_WhenUpdateIsSuccess()
            {
                // NOTICE, we do not test if anything actually changed on the DB,
                // we only test that the returned values match the submitted values
                // Arrange
                // Creating a new category request
                CategoryRequest categoryRequest = new CategoryRequest
                {
                    CategoryName = "Kids"
                };

                // Setting up the category ID for testing
                int categoryId = 1;

                // Creating a mock category with existing values
                Category category = new Category
                {
                    Id = categoryId,
                    CategoryName = "Kids"
                };

                // Setting up the mock category repository to return the mock category
                _mockCategoryRepository
                    .Setup(x => x.UpdateExistingCategory(It.IsAny<int>(), It.IsAny<Category>()))
                    .ReturnsAsync(category);

                // Act
                var result = await _categoryService.UpdateCategory(categoryId, categoryRequest);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<CategoryResponse>(result);
                Assert.Equal(categoryId, result.Id);
                Assert.Equal(categoryRequest.CategoryName, result.CategoryName);
            }

            [Fact]
            public async void UpdateCategory_ShouldReturnNull_WhenCategoryDoesNotExist()
            {
                // Arrange
                // Creating a new category request
                CategoryRequest categoryRequest = new CategoryRequest
                {
                    CategoryName = "Kids"
                };

                // Setting up the category ID for testing
                int categoryId = 1;

                // Setting up the mock category repository to return null
                _mockCategoryRepository
                    .Setup(x => x.UpdateExistingCategory(It.IsAny<int>(), It.IsAny<Category>()))
                    .ReturnsAsync(() => null);

                // Act
                var result = await _categoryService.UpdateCategory(categoryId, categoryRequest);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async void DeleteCategory_ShouldReturnCategoryResponse_WhenDeleteIsSuccess()
            {
                // Arrange
                // Setting up the category ID for deletion
                int categoryId = 1;

                // Creating a mock category for the deleted category
                Category deletedCategory = new Category
                {
                    Id = 1,
                    CategoryName = "Kids"
                };

                // Setting up the mock category repository to return the deleted category
                _mockCategoryRepository
                    .Setup(x => x.DeleteCategoryById(It.IsAny<int>()))
                    .ReturnsAsync(deletedCategory);

                // Act
                var result = await _categoryService.DeleteCategory(categoryId);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<CategoryResponse>(result);
                Assert.Equal(categoryId, result.Id);
            }

            [Fact]
            public async void DeleteCategory_ShouldReturnNull_WhenCategoryDoesNotExist()
            {
                // Arrange
                // Setting up the category ID for deletion
                int categoryId = 1;

                // Setting up the mock category repository to return null
                _mockCategoryRepository
                    .Setup(x => x.DeleteCategoryById(It.IsAny<int>()))
                    .ReturnsAsync(() => null);

                // Act
                var result = await _categoryService.DeleteCategory(categoryId);

                // Assert
                Assert.Null(result);
            }
        }

    }


