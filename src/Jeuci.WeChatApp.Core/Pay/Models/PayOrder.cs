namespace Jeuci.WeChatApp.Pay.Models
{
    public class PayOrder
    {
        public string ID { get; set; }

        public int UId { get; set; }

        public double Cost { get; set; }

        public int GoodsType { get; set; }

        public int? GoodsID { get; set; }

        public string GoodsName { get; set; }

        public string GoodsInfo { get; set; }

        public int PayType { get; set; }

        public int PayMode { get; set; }

        public string PayAppID { get; set; }

        public string PayOrderID { get; set; }

        public string PayExtendInfo { get; set; }

        public string PayState { get; set; }

        public string Remarks { get; set; }

    }
}