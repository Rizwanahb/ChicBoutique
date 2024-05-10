namespace H6_ChicBotique.DTOs
{
    public class ProductResponse
    { //It is a output for the corresponding request
        public int Id { get; set; } // Unique identifier for the product
        public string Title { get; set; }
        public decimal Price { get; set; } 
        public string Description { get; set; } 
        public string Image { get; set; } 
        public int Stock { get; set; } 
        public int CategoryId { get; set; } // Unique identifier of the category to which the product belongs

        public ProductCategoryResponse Category { get; set; } // Information about the category associated with the product
    }

    public class ProductCategoryResponse
    {
        public int Id { get; set; } // Unique identifier for the category
        public string CategoryName { get; set; } 
    }

}
