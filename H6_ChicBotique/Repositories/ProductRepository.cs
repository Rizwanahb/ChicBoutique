using H6_ChicBotique.Database;
using H6_ChicBotique.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace H6_ChicBotique.Repositories
{
    // Interface for product repository
    public interface IProductRepository
    {
        Task<List<Product>> SelectAllProducts(); // Get all products
        Task<Product> SelectProductById(int productId); // Get a product by ID
        Task<List<Product>> SelectProductsByCategoryId(int categoryId); // Get products by category ID
        Task<Product> InsertNewProduct(Product product); // Insert a new product
        Task<Product> UpdateExistingProduct(int productId, Product product); // Update an existing product
        Task<Product> DeleteProductById(int productId); // Delete a product by ID
    }

    // Implementation of IProductRepository interface
    public class ProductRepository : IProductRepository
    {
        private readonly ChicBotiqueDatabaseContext _context; // Database context for product operations

        // Constructor with dependency injection
        public ProductRepository(ChicBotiqueDatabaseContext context)
        {
            _context = context;
        }

        // Get all products, including associated category information
        public async Task<List<Product>> SelectAllProducts()
        {
            return await _context.Product
                .Include(a => a.Category)
                .OrderBy(a => a.CategoryId)

                .ToListAsync();
        }
        // Get all products by Id
        public async Task<Product> SelectProductById(int product_Id)
        {
            return await _context.Product
                .Include(a => a.Category)
                .OrderBy(a => a.CategoryId)
                .FirstOrDefaultAsync(product => product.Id == product_Id);
        }


        // Retrieve a list of products based on the given category ID
        public async Task<List<Product>> SelectProductsByCategoryId(int Category_Id)
        {

            return await _context.Product
                .Include(a => a.Category)
                .OrderBy(a => a.CategoryId)
                .Where(a => a.CategoryId==Category_Id)
                .ToListAsync();
        }

        // Insert a new product into the database
        public async Task<Product> InsertNewProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        // Update an existing product in the database based on its ID
        public async Task<Product> UpdateExistingProduct(int product_Id, Product product)
        {
            Product updateProduct = await _context.Product.FirstOrDefaultAsync(product => product.Id == product_Id);

            if (updateProduct != null)
            {
                updateProduct.Title = product.Title;
                updateProduct.Price = product.Price;
                updateProduct.Description = product.Description;
                updateProduct.Image = product.Image;
                updateProduct.Stock = product.Stock;

                

                await _context.SaveChangesAsync();

            }

            return updateProduct;
        }

        // Delete a product from the database based on its ID
        public async Task<Product> DeleteProductById(int product_Id)
        {
            // Find the product to delete by its ID
            Product deleteProduct = await _context.Product.FirstOrDefaultAsync(product => product.Id == product_Id);

            if (deleteProduct != null)
            {
                _context.Product.Remove(deleteProduct);        // Remove the product from the database             
                await _context.SaveChangesAsync();   // Save changes to the database
            }
            return deleteProduct;
        }

    }

}
