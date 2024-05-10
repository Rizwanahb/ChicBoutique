using H6_ChicBotique.Database;
using H6_ChicBotique.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace H6_ChicBotique.Repositories
{
    // Interface for category repository
    public interface ICategoryRepository
    {
        Task<List<Category>> SelectAllCategories(); // Get all categories including associated products
        Task<Category> SelectCategoryById(int categoryId); // Get a category by ID including associated products
        Task<List<Category>> SelectAllCategoriesWithoutProducts(); // Get all categories without associated products
        Task<Category> InsertNewCategory(Category category); // Insert a new category
        Task<Category> UpdateExistingCategory(int categoryId, Category category); // Update an existing category
        Task<Category> DeleteCategoryById(int categoryId); // Delete a category by ID
    }

    // Implementation of ICategoryRepository interface
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ChicBotiqueDatabaseContext _context; // Database context for category operations

        // Constructor with dependency injection
        public CategoryRepository(ChicBotiqueDatabaseContext context)
        {
            _context = context;
        }

        // Get all categories including associated products
        public async Task<List<Category>> SelectAllCategories()
        {
            return await _context.Category
                .Include(b => b.Products) // Include associated products
                .ToListAsync();
        }

        // Get a category by ID including associated products
        public async Task<Category> SelectCategoryById(int categoryId)
        {
            return await _context.Category
                .Include(a => a.Products) // Include associated products
                .FirstOrDefaultAsync(category => category.Id == categoryId);
        }

        // Get all categories without associated products
        public async Task<List<Category>> SelectAllCategoriesWithoutProducts()
        {
            return await _context.Category
                .ToListAsync();
        }

        // Insert a new category
        public async Task<Category> InsertNewCategory(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        // Update an existing category
        public async Task<Category> UpdateExistingCategory(int categoryId, Category category)
        {
            Category updateCategory = await _context.Category.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (updateCategory != null)
            {
                // Update category properties
                updateCategory.CategoryName = category.CategoryName;

                await _context.SaveChangesAsync();
            }
            return updateCategory;
        }

        // Delete a category by ID
        public async Task<Category> DeleteCategoryById(int categoryId)
        {
            Category deleteCategory = await _context.Category.FirstOrDefaultAsync(category => category.Id == categoryId);
            if (deleteCategory != null)
            {
                _context.Remove(deleteCategory);
                await _context.SaveChangesAsync();
            }
            return deleteCategory;
        }
    }

}
