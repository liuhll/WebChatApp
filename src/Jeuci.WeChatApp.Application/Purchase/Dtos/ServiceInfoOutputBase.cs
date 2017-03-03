namespace Jeuci.WeChatApp.Purchase.Dtos
{
    public class ServiceInfoOutputBase
    {
        public string OrderId { get; set; }

        public string ServiceName { get; set; }
     
        public decimal OrderPrice { get; set; }

        public string Description { get; set; }
       
        public string Sid { get; set; }

        public string PriceId { get; set; }
    }
}