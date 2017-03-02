namespace Jeuci.WeChatApp.Pay.Models
{
    public class AlipayOrderOptions
    {
        public string out_trade_no { get; set; }

        public string subject { get; set; }

        public string seller_id { get; set; }

        public string total_amount { get; set; }
    }
}