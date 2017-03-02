namespace Jeuci.WeChatApp.Purchase.Dtos
{
    public class PayOrderInputBase
    {
        public string ID { get; set; }

        public double Cost { get; set; }

        public string GoodsInfo { get; set; }

        public string GoodsName { get; set; }

        public int? GoodsId { get; set; }

        public int GoodType { get; set; }
    }
}