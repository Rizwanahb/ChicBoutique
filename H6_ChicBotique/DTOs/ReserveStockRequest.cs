using System.ComponentModel.DataAnnotations;

namespace H6_ChicBotique.DTOs
{
    public class ReserveStockRequest
    { //Input from the clientside  page
        [Required]
        public string clientBasketId { get; set; }
        [Required]
        public int productId { get; set; }
        [Required]
        public int amountToReserve { get; set; }
    }
}
