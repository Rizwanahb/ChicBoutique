// Importing necessary namespaces
using Xunit;
using Microsoft.EntityFrameworkCore;
using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.Repositories;
using H6_ChicBotique.Database;

namespace H6_ChicBotiqueTestProject.RepositoryTests
{
    

    // Test class for ProductRepository
    public class ProductRepositoryTests
    {
        // Private fields to store testing environment and objects
        private readonly DbContextOptions<ChicBotiqueDatabaseContext> _options;
        private readonly ChicBotiqueDatabaseContext _context;
        private readonly ProductRepository _productRepository;

        // Constructor to set up the testing environment
        public ProductRepositoryTests()
        {
            // Setting up in-memory database options
            _options = new DbContextOptionsBuilder<ChicBotiqueDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ChicBotique")
                .Options;

            // Creating an instance of the context and repository
            _context = new(_options);
            _productRepository = new(_context);
        }

        // Test case for SelectAllProducts method
        [Fact]
        public async void SelectAllProducts_ShouldReturnListOfProducts_WhenProductExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Adding a category and products to the in-memory database
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

            _context.Product.Add(new Product
            {
                Id = 1,
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1
            });
            _context.Product.Add(new Product
            {
                Id = 2,
                Title = "Blue T-Shirt",
                Price = 199.99M,
                Description = "T-Shirt for men",
                Image = "BlueTShirt.jpg",
                Stock = 10,
                CategoryId = 2
            });     
            _context.Product.Add(new Product
            {
                Id = 6,
                Title = "Long dress",
                Price = 299.99M,
                Description = "Summer clothing",
                Image = "floral-dress.jpg",
                Stock = 10,
                CategoryId = 3
            });
           


            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepository.SelectAllProducts();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Product>>(result);
            Assert.Equal(3, result.Count);
        }

        // Additional test cases for other methods...

        // Test case for SelectAllProducts method
        [Fact]
        public async void SelectAllProducts_ShouldReturnEmptyListOfProducts_WhenNoProductExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _productRepository.SelectAllProducts();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Product>>(result);
            Assert.Empty(result);
        }

        // Test case for SelectProductById method
        [Fact]
        public async void SelectProductById_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int productId = 1;

            _context.Category.Add(new Category
            {
                Id = 1,
                CategoryName = "Kids"
            });

            _context.Product.Add(new Product
            {
                Id = 1,
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepository.SelectProductById(productId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(productId, result.Id);
        }

        // Test case for SelectProductById method
        [Fact]
        public async void SelectProductById_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _productRepository.SelectProductById(1);

            // Assert
            Assert.Null(result);
        }

        // Test case for InsertNewProduct method
        [Fact]
        public async void InsertNewProduct_ShouldAddNewIdToProduct_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int expectedNewId = 1;

            Product product = new()
            {
                Id = 1,
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1
            };

            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepository.InsertNewProduct(product);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(expectedNewId, result.Id);
        }

        // Test case for InsertNewProduct method when Product already exists
        [Fact]
        public async void InsertNewProduct_ShouldFailToAddNewProduct_WhenProductIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Product product = new()
            {
                Id = 1,
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1
            };

            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            // Act
            async Task action() => await _productRepository.InsertNewProduct(product);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        // Test case for UpdateExistingProduct method
        [Fact]
        public async void UpdateExistingProduct_ShouldChangeValuesOnProduct_WhenProductExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int productId = 1;

            // Adding a product to the in-memory database
            Product newProduct = new()
            {
                Id = 1,
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1
            };

            _context.Product.Add(newProduct);
            await _context.SaveChangesAsync();

            // Creating an updated product
            Product updateProduct = new()
            {
                Id = productId,
                Title = "updated Fancy dress",
                Price = 299.99M,
                Description = "updated Kids dress",
                Image = "updatedress1.jpg",
                CategoryId = 1,
                Stock = 10
            };

            // Act
            var result = await _productRepository.UpdateExistingProduct(productId, updateProduct);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal(updateProduct.Title, result.Title);
            Assert.Equal(updateProduct.Price, result.Price);
            Assert.Equal(updateProduct.Description, result.Description);
            Assert.Equal(updateProduct.Image, result.Image);
            Assert.Equal(updateProduct.CategoryId, result.CategoryId);
            Assert.Equal(updateProduct.Stock, result.Stock);
        }

        // Test case for UpdateExistingProduct method
        [Fact]
        public async void UpdateExistingProduct_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int productId = 1;

            // Creating an update product
            Product updateProduct = new()
            {
                Id = 1,
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1
            };

            // Act
            var result = await _productRepository.UpdateExistingProduct(productId, updateProduct);

            // Assert
            Assert.Null(result);
        }

        // Test case for DeleteProductById method
        [Fact]
        public async void DeleteProductById_ShouldReturnDeleteProduct_WhenProductIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int productId = 1;

            // Adding a product to the in-memory database
            Product newProduct = new()
            {
                Id = 1,
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1
            };

            _context.Product.Add(newProduct);
            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepository.DeleteProductById(productId);
            var product = await _productRepository.SelectProductById(productId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(productId, result.Id);
            Assert.Null(product);
        }

        // Test case for DeleteProductById method when product does not exist
        [Fact]
        public async void DeleteProductById_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _productRepository.DeleteProductById(1);

            // Assert
            Assert.Null(result);
        }


    }

}
