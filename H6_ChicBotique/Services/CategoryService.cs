using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Repositories;

namespace H6_ChicBotique.Services
{
    public interface ICategoryService // Interface definition for Category service
    {
        Task<List<CategoryResponse>> GetAllCategories(); // Method to retrieve all Categories as CategoryResponse objects
        Task<CategoryResponse> GetCategoryById(int categoryId); // Method to retrieve a Category by ID as a CategoryResponse object
        Task<List<CategoryResponse>> GetAllCategoriesWithoutProducts();
        Task<CategoryResponse> CreateCategory(CategoryRequest newCategory);
        Task<CategoryResponse> UpdateCategory(int category_Id, CategoryRequest updateCategory);
        Task<CategoryResponse> DeleteCategory(int category_Id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryResponse>> GetAllCategories()
        {
            // Retrieve all categories from the repository
            List<Category> categories = await _categoryRepository.SelectAllCategories();

            // If categories exist, map each category to a CategoryResponse and return the list
            if (categories != null)
            {
                return categories.Select(category => MapCategoryToCategoryResponse(category)).ToList();
            }

            return null;
        }

        public async Task<CategoryResponse> GetCategoryById(int categoryId)
        {
            // Retrieve a category by ID from the repository
            Category category = await _categoryRepository.SelectCategoryById(categoryId);

            // If the category exists, map it to a CategoryResponse and return
            if (category != null)
            {
                return MapCategoryToCategoryResponse(category);
            }

            return null;
        }

        public async Task<List<CategoryResponse>> GetAllCategoriesWithoutProducts()
        {
            // Retrieve categories without products from the repository
            List<Category> categories = await _categoryRepository.SelectAllCategoriesWithoutProducts();

            // If categories exist, map each category to a CategoryResponse and return the list
            if (categories != null)
            {
                return categories.Select(category => MapCategoryToCategoryResponse(category)).ToList();
            }

            return null;
        }

        public async Task<CategoryResponse> CreateCategory(CategoryRequest newCategory)
        {
            // Map CategoryRequest to a Category entity
            Category category = MapCategoryRequestToCategory(newCategory);

            // Insert the new category into the repository
            Category insertedCategory = await _categoryRepository.InsertNewCategory(category);

            // If the insertion is successful, map the inserted category to a CategoryResponse and return
            if (insertedCategory != null)
            {
                return MapCategoryToCategoryResponse(insertedCategory);
            }

            return null;
        }

        public async Task<CategoryResponse> UpdateCategory(int category_Id, CategoryRequest updateCategory)
        {
            // Map CategoryRequest to a Category entity
            Category category = MapCategoryRequestToCategory(updateCategory);

            // Update the existing category in the repository
            Category updatedCategory = await _categoryRepository.UpdateExistingCategory(category_Id, category);

            // If the update is successful, map the updated category to a CategoryResponse and return
            if (updatedCategory != null)
            {
                return MapCategoryToCategoryResponse(updatedCategory);
            }

            return null;
        }

        public async Task<CategoryResponse> DeleteCategory(int category_Id)
        {
            // Delete the category by ID from the repository
            Category deletedCategory = await _categoryRepository.DeleteCategoryById(category_Id);

            // If the deletion is successful, map the deleted category to a CategoryResponse and return
            if (deletedCategory != null)
            {
                return MapCategoryToCategoryResponse(deletedCategory);
            }

            return null;
        }

        // Helper method to map a CategoryRequest to a Category entity
        public static Category MapCategoryRequestToCategory(CategoryRequest category)
        {
            return new Category()
            {
                CategoryName = category.CategoryName,
            };
        }

        // Helper method to map a Category to a CategoryResponse
        private CategoryResponse MapCategoryToCategoryResponse(Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Products = category.Products.Select(product => new CategoryProductResponse
                {
                    Id = product.Id,
                    Title = product.Title,
                    Price = product.Price,
                    Description = product.Description,
                    Image = product.Image,
                    Stock = product.Stock
                }).ToList()
            };
        }
    }

}
