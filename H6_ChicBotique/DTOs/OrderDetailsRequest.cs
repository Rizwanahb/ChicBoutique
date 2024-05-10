namespace H6_ChicBotique.DTOs
{
    //Input from the client for ordering 
    public class OrderDetailsRequest
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public decimal ProductPrice { get; set; }



        public int Quantity { get; set; }



    }
}
