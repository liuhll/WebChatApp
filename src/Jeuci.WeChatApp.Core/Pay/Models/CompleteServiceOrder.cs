namespace Jeuci.WeChatApp.Pay.Models
{
    public class CompleteServiceOrder
    {

        public string OrderID { get; set; }

        public double Cost { get; set; }

        public string NewID { get; set; }

        public string Remarks { get; set; }

        public string PayOrderID { get; set; }

        public string PayExtendInfo { get; set; }

        public string PayState { get; set; }
    }
}