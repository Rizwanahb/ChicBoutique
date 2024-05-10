namespace H6_ChicBotique.DTOs
{
    public class CategoryResponse
    {
        public int Id { get; set; } // Unique identifier for the category
        public string CategoryName { get; set; } // Name of the category

        public List<CategoryProductResponse> Products { get; set; } = new(); // List of products associated with the category
    }

    public class CategoryProductResponse
    {
        public int Id { get; set; } // Unique identifier for the product
        public string Title { get; set; } // Title or name of the product
        public decimal Price { get; set; } // Price of the product
        public string Description { get; set; } // Description of the product
        public string Image { get; set; } // Image URL or path for the product
        public int Stock { get; set; } // Stock quantity of the product
    }


}
