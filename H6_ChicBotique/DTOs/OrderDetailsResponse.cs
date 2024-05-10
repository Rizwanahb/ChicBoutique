namespace H6_ChicBotique.DTOs
{
    public class OrderDetailsResponse
    {
        //It is a output for the corresponding request

        public int Id { get; set; }


        public int ProductId { get; set; }
        public string ProductTitle { get; set; }

        // public ProductResponse Product { get; set; }

        public decimal ProductPrice { get; set; }



        public int Quantity { get; set; }

    }
}
