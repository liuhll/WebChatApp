
namespace Jeuci.WeChatApp.Purchase.Dtos
{
    public class PayOrderInput
    {
        public string ID { get; set; }

        public string OpenId { get; set; }

        public double Cost { get; set; }

        public string GoodsInfo { get; set; }

        public string GoodsName { get; set; }

        public int GoodsId { get; set; }

    }
}