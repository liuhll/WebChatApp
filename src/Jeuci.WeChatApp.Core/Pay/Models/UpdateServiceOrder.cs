namespace Jeuci.WeChatApp.Pay.Models
{
    public class UpdateServiceOrder
    {
        public string ID { get; set; }

        public string PayOrderID { get; set; }

        public string PayExtendInfo { get; set; }

        public string PayState { get; set; }

        public int? State { get; set; }

    }
}