using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Repositories;
using H6_ChicBotique.Services;
using Moq;


namespace H6_ChicBotiqueTestProject.ServiceTests
{
    // Test class for ProductService
    public class ProductServiceTests
    {
        // Private fields to store testing environment and objects
        private readonly ProductService _productService;
        private readonly Mock<IProductRepository> _mockProductRepository = new();
        private readonly Mock<ICategoryRepository> _mockCategoryRepository = new();

        // Constructor to set up the testing environment
        public ProductServiceTests()
        {
            _productService = new ProductService(_mockProductRepository.Object, _mockCategoryRepository.Object);
        }

        // Test case for GetAllProducts method
        [Fact]
        public async void GetAllProducts_ShouldReturnListOfProductResponses_WhenProductsExist()
        {
            // Arrange
            List<Product> products = new();
            Category newCategory = new()
            {
                Id = 1,
                CategoryName = "Kids"
            };

            products.Add(new()
            {
                Id = 1,
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1,
                Category = newCategory
            });

            products.Add(new()
            {
                Id = 2,
                Title = "Blue T-Shirt",
                Price = 199.99M,
                Description = "T-Shirt for men",
                Image = "BlueTShirt.jpg",
                Stock = 10,
                CategoryId = 2,
                Category = newCategory
            });
            products.Add(new()
            {
                Id = 6,
                Title = "Long dress",
                Price = 299.99M,
                Description = "Summer clothing",
                Image = "floral-dress.jpg",
                Stock = 10,
                CategoryId = 3,
                Category = newCategory
            });
            _mockProductRepository
                .Setup(x => x.SelectAllProducts())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.IsType<List<ProductResponse>>(result);
        }
        // Test case for GetAllProducts method when no products exist
        [Fact]
        public async void GetAllProducts_ShouldReturnEmptyListOfProductResponses_WhenNoProductsExist()
        {
            // Arrange
            List<Product> products = new();

            _mockProductRepository
                .Setup(x => x.SelectAllProducts())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsType<List<ProductResponse>>(result);
        }

        // Test case for GetProductById method when product exists
        [Fact]
        public async void GetProductById_ShouldReturnProductResponse_WhenProductExists()
        {
            // Arrange
            int productId = 1;
            Category newCategory = new()
            {
                Id = 1,
                CategoryName = "Kids"
            };

            Product product = new()
            {
                Id = 1,
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1,
                Category = newCategory
            };

            _mockProductRepository
                .Setup(x => x.SelectProductById(It.IsAny<int>()))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductById(productId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductResponse>(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Title, result.Title);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.Description, result.Description);
            Assert.Equal(product.Image, result.Image);
            Assert.Equal(product.Stock, result.Stock);
            Assert.Equal(product.CategoryId, result.CategoryId);
        }

        // Test case for GetProductById method when product does not exist
        [Fact]
        public async void GetProductById_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            int productId = 1;

            _mockProductRepository
                .Setup(x => x.SelectProductById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _productService.GetProductById(productId);

            // Assert
            Assert.Null(result);
        }
        // Test case for CreateProduct method when product creation is successful
        [Fact]
        public async void CreateProduct_ShouldReturnProductResponse_WhenCreateIsSuccess()
        {
            // Arrange
            Category newCategory = new()
            {
                Id = 1,
                CategoryName = "Kids"
            };

            ProductRequest newProduct = new()
            {
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1
            };

            int productId = 1;

            Product createdProduct = new()
            {
                Id = productId,
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1,
                Category = null
            };

            _mockProductRepository
                .Setup(x => x.InsertNewProduct(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);
            _mockCategoryRepository
                .Setup(x => x.SelectCategoryById(It.IsAny<int>()))
                .ReturnsAsync(newCategory);

            // Act
            var result = await _productService.CreateProduct(newProduct);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductResponse>(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal(newProduct.Title, result.Title);
            Assert.Equal(newProduct.Price, result.Price);
            Assert.Equal(newProduct.Description, result.Description);
            Assert.Equal(newProduct.Image, result.Image);
            Assert.Equal(newProduct.Stock, result.Stock);
            Assert.Equal(newProduct.CategoryId, result.CategoryId);
        }

        // Test case for CreateProduct method when repository returns null
        [Fact]
        public async void CreateProduct_ShouldReturnNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            Category newCategory = new()
            {
                Id = 1,
                CategoryName = "Toy"
            };
            ProductRequest newProduct = new()
            {
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1
            };

            _mockProductRepository
                .Setup(x => x.InsertNewProduct(It.IsAny<Product>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _productService.CreateProduct(newProduct);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateProduct_ShouldReturnProductResponse_WhenUpdateIsSuccess()
        {
            // We do not test if anything actually changed on the DB,
            // we only test that the returned values match the submitted values

            // Arrange
            // Creating a mock category for the product
            Category newCategory = new()
            {
                Id = 1,
                CategoryName = "Kids"
            };

            // Creating a product request with updated values
            ProductRequest productRequest = new()
            {
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1
            };

            // Creating a product with existing values
            int productId = 1;
            Product product = new()
            {
                Id = productId,
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1,
                Category = null
            };

            // Setting up the mock product repository to return the updated product
            _mockProductRepository
                .Setup(x => x.UpdateExistingProduct(It.IsAny<int>(), It.IsAny<Product>()))
                .ReturnsAsync(product);

            // Setting up the mock category repository to return the mock category
            _mockCategoryRepository
                .Setup(x => x.SelectCategoryById(It.IsAny<int>()))
                .ReturnsAsync(newCategory);

            // Act
            var result = await _productService.UpdateProduct(productId, productRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductResponse>(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal(productRequest.Title, result.Title);
            Assert.Equal(productRequest.Description, result.Description);
            Assert.Equal(productRequest.Image, result.Image);
            Assert.Equal(productRequest.Stock, result.Stock);
            Assert.Equal(productRequest.CategoryId, result.CategoryId);
        }

        [Fact]
        public async void UpdateProduct_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            // Creating a product request with updated values
            ProductRequest productRequest = new()
            {
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1
            };

            int productId = 1;

            // Setting up the mock product repository to return null, indicating product not found
            _mockProductRepository
                .Setup(x => x.UpdateExistingProduct(It.IsAny<int>(), It.IsAny<Product>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _productService.UpdateProduct(productId, productRequest);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async void DeleteProduct_ShouldReturnProductResponse_WhenDeleteIsSuccess()
        {
            // Arrange
            // Setting up the product ID for the deletion
            int productId = 1;

            // Creating a mock category for the product
            Category newCategory = new()
            {
                Id = 1,
                CategoryName = "Kids"
            };

            // Creating a deleted product with existing values
            Product deletedProduct = new()
            {
                Id = productId, // Adding the product ID to match the expected value
                Title = "Fancy dress",
                Price = 299.99M,
                Description = "Kids dress",
                Image = "dress1.jpg",
                Stock = 10,
                CategoryId = 1,
                Category = null
            };

            // Setting up the mock product repository to return the deleted product
            _mockProductRepository
                .Setup(x => x.DeleteProductById(It.IsAny<int>()))
                .ReturnsAsync(deletedProduct);

            // Setting up the mock category repository to return the mock category
            _mockCategoryRepository
                .Setup(x => x.SelectCategoryById(It.IsAny<int>()))
                .ReturnsAsync(newCategory);

            // Act
            var result = await _productService.DeleteProduct(productId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductResponse>(result);
            Assert.Equal(productId, result.Id);
        }

        [Fact]
        public async void DeleteProduct_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            // Setting up the product ID for the deletion
            int productId = 1;

            // Setting up the mock product repository to return null, indicating product not found
            _mockProductRepository
                .Setup(x => x.DeleteProductById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _productService.DeleteProduct(productId);

            // Assert
            Assert.Null(result);
        }




    }

}
