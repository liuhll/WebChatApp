namespace Jeuci.WeChatApp.Pay.Models
{
    public class AliPayOrder
    {
        public string ID { get; set; }

        public double Cost { get; set; }

        public string GoodsInfo { get; set; }

        public string GoodsName { get; set; }

        public int? GoodsId { get; set; }

        public int GoodType { get; set; }

        public int Uid { get; set; }
    }
}