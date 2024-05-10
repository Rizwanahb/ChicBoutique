using H6_ChicBotique.Controllers;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;


namespace H6_ChicBotiqueTestProject.ControllerTests
{
       
        /// Unit tests for the "CategoryController" class.     
        public class CategoryControllerTests
        {
            private readonly CategoryController _categoryController;
            private readonly Mock<ICategoryService> _mockCategoryService = new();

            public CategoryControllerTests()
            {
                // Initialize the CategoryController with a mocked ICategoryService
                _categoryController = new CategoryController(_mockCategoryService.Object);
            }

            /// Verifies that GetAll returns StatusCode 200 when categories exist.       
            [Fact]
            public async void GetAllCategories_ShouldReturnStatusCode200_WhenCategoriesExist()
            {
                // Arrange
                List<CategoryResponse> categories = new List<CategoryResponse>
            {
                new CategoryResponse { Id = 1, CategoryName = "Kids" },
                new CategoryResponse { Id = 2, CategoryName = "Men" },
                new CategoryResponse { Id = 3, CategoryName = "Women" },
            };

                _mockCategoryService.Setup(x => x.GetAllCategories()).ReturnsAsync(categories);

                // Act
                var result = await _categoryController.GetAll();

                // Assert
                var statusCodeResult = (IStatusCodeActionResult)result;
                Assert.Equal(200, statusCodeResult.StatusCode);
            }

           
            /// Verifies that GetAll returns StatusCode 204 when no categories exist.
            [Fact]
            public async void GetAll_ShouldReturnStatusCode204_WhenNoCategoriesExist()
            {
                // Arrange
                List<CategoryResponse> products = new List<CategoryResponse>();

                _mockCategoryService.Setup(x => x.GetAllCategories()).ReturnsAsync(products);

                // Act
                var result = await _categoryController.GetAll();

                // Assert
                var statusCodeResult = (IStatusCodeActionResult)result;
                Assert.Equal(204, statusCodeResult.StatusCode);
            }

        // Verifies that GetAll returns StatusCode 500 when null is returned from the category service.
        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenNullIsReturnedFromService()
        {
            // Arrange
            _mockCategoryService.Setup(x => x.GetAllCategories()).ReturnsAsync(() => null);

            // Act
            var result = await _categoryController.GetAll();

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        // Verifies that GetAll returns StatusCode 500 when an exception is raised during category retrieval.
        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _mockCategoryService.Setup(x => x.GetAllCategories()).ReturnsAsync(() => throw new System.Exception("This is an exception"));

            // Act
            var result = await _categoryController.GetAll();

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        // Verifies that GetById returns StatusCode 200 when category data exists.
        [Fact]
        public async void GetCategoryById_ShouldReturnStatusCode200_WhenDataExists()
        {
            // Arrange
            int categoryId = 1;
            CategoryResponse category = new CategoryResponse { Id = 1, CategoryName = "Kids" };

            _mockCategoryService.Setup(x => x.GetCategoryById(It.IsAny<int>())).ReturnsAsync(category);

            // Act
            var result = await _categoryController.GetById(categoryId);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        // Verifies that GetById returns StatusCode 404 when the requested category does not exist.
        [Fact]
        public async void GetById_ShouldReturnStatusCode404_WhenCategoryDoesNotExist()
        {
            // Arrange
            int categoryId = 1;
            _mockCategoryService.Setup(x => x.GetCategoryById(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var result = await _categoryController.GetById(categoryId);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        // Verifies that GetById returns StatusCode 500 when an exception is raised during category retrieval.
        [Fact]
        public async void GetById_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _mockCategoryService.Setup(x => x.GetCategoryById(It.IsAny<int>()))
                .ReturnsAsync(() => throw new System.Exception("This is an Exception"));

            // Act
            var result = await _categoryController.GetById(1);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        // Verifies that Create returns StatusCode 200 when a category is successfully created.
        [Fact]
        public async void CreateCategory_ShouldReturnStatusCode200_WhenCategoryIsSuccessfullyCreated()
        {
            // Arrange
            CategoryRequest newCategory = new CategoryRequest { CategoryName = "Kids" };
            CategoryResponse categoryResponse = new CategoryResponse 
            { Id = 1,
             CategoryName = "Kids" };

            _mockCategoryService.Setup(x => x.CreateCategory(It.IsAny<CategoryRequest>()))
                .ReturnsAsync(categoryResponse);

            // Act
            var result = await _categoryController.Create(newCategory);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        // Verifies that Create returns StatusCode 500 when an exception is raised during category creation.
        [Fact]
        public async void Create_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            CategoryRequest newCategory = new CategoryRequest { CategoryName = "Kids" };

            _mockCategoryService.Setup(x => x.CreateCategory(It.IsAny<CategoryRequest>()))
                .ReturnsAsync(() => throw new System.Exception("This is an exception"));

            // Act
            var result = await _categoryController.Create(newCategory);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        // Verifies that Update returns StatusCode 200 when a category is successfully updated.
        [Fact]
        public async void UpdateCategory_ShouldReturnStatusCode200_WhenCategoryIsSuccessfullyUpdated()
        {
            // Arrange
            CategoryRequest updateCategory = new CategoryRequest { CategoryName = "Kids" };
            int categoryId = 1;
            CategoryResponse categoryResponse = new CategoryResponse { Id = 1, CategoryName = "Kids" };

            _mockCategoryService.Setup(x => x.UpdateCategory(It.IsAny<int>(), It.IsAny<CategoryRequest>())).ReturnsAsync(categoryResponse);

            // Act
            var result = await _categoryController.Update(categoryId, updateCategory);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        // Verifies that Update returns StatusCode 404 when trying to update a category that does not exist.
        [Fact]
        public async void Update_ShouldReturnStatusCode404_WhenTryingToUpdateCategoryWhichDoesNotExist()
        {
            // Arrange
            CategoryRequest updateCategory = new CategoryRequest { CategoryName = "Kids" };

            _mockCategoryService.Setup(x => x.UpdateCategory(It.IsAny<int>(), It.IsAny<CategoryRequest>()))
                .ReturnsAsync(() => null);

            // Act
            int categoryId = 1;
            var result = await _categoryController.Update(categoryId, updateCategory);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        // Verifies that Update returns StatusCode 500 when an exception is raised during category update.
        [Fact]
        public async void Update_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            CategoryRequest updateCategory = new CategoryRequest { CategoryName = "Kids" };
            int categoryId = 1;

            _mockCategoryService.Setup(x => x.UpdateCategory(It.IsAny<int>(), It.IsAny<CategoryRequest>()))
                .ReturnsAsync(() => throw new System.Exception("This is an exception"));

            // Act
            var result = await _categoryController.Update(categoryId, updateCategory);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        // Verifies that Delete returns StatusCode 200 when a category is successfully deleted.
        [Fact]
        public async void DeleteCategory_ShouldReturnStatusCode200_WhenCategoryIsDeleted()
        {
            // Arrange
            int categoryId = 1;
            CategoryResponse categoryResponse = new CategoryResponse { Id = 1, CategoryName = "Kids" };

            _mockCategoryService.Setup(x => x.DeleteCategory(It.IsAny<int>())).ReturnsAsync(categoryResponse);

            // Act
            var result = await _categoryController.Delete(categoryId);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        /// Verifies that Delete returns StatusCode 404 when trying to delete a category that does not exist.

        [Fact]
            public async void Delete_ShouldReturnStatusCode404_WhenTryingToDeleteCategoryWhichDoesNotExist()
            {
                // Arrange
                int categoryId = 1;

                _mockCategoryService.Setup(x => x.DeleteCategory(It.IsAny<int>()))
                .ReturnsAsync(() => null);

                // Act
                var result = await _categoryController.Delete(categoryId);

                // Assert
                var statusCodeResult = (IStatusCodeActionResult)result;
                Assert.Equal(404, statusCodeResult.StatusCode);
            }

            // Verifies that Delete returns StatusCode 500 when an exception is raised during category deletion.
        
            [Fact]
            public async void Delete_ShouldReturnStatusCode500_WhenExceptionIsRaised()
            {
                // Arrange
                int categoryId = 1;

                _mockCategoryService.Setup(x => x.DeleteCategory(It.IsAny<int>())).ReturnsAsync(() => throw new System.Exception("This is an exception"));

                // Act
                var result = await _categoryController.Delete(categoryId);

                // Assert
                var statusCodeResult = (IStatusCodeActionResult)result;
                Assert.Equal(500, statusCodeResult.StatusCode);
            }
        }
    }

