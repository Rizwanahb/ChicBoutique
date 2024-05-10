using System.ComponentModel.DataAnnotations;

namespace H6_ChicBotique.DTOs
{
    public class OrderAndPaymentRequest
    {
        public DateTime OrderDate { get; set; }

        [Required]

        public Guid AccountInfoId { get; set; }
        [Required]

        public string ClientBasketId { get; set; }
        [Required]
        public string? Status { get; set; }

        [Required]
        public string? TransactionId { get; set; }

        [Required]
        public string? PaymentMethod { get; set; }

        [Required]
      
        public decimal Amount { get; set; }
        [Required]
        public DateTime TimePaid { get; set; }
        [Required]
        public ShippingDetailsRequest shippingDetails { get; set; }

        [Required]
        public List<OrderDetailsRequest> OrderDetails { get; set; }
    }
}
