using System.ComponentModel.DataAnnotations;

namespace H6_ChicBotique.DTOs
{
    //Input from the clientside or admin page 
    public class ProductRequest
    {
        //The [Required] attribute indicates that property is mandatory, and a value must be provided.
        [Required]
        public string Title { get; set; } 

        [Required]
        public decimal Price { get; set; } 

        [Required]
        public string Description { get; set; } 

        [Required]
        public string Image { get; set; } 

        [Required]
        public int Stock { get; set; } 

        [Required]
        public int CategoryId { get; set; } // The unique identifier of the category to which the product belongs, marked as required
    }

}
