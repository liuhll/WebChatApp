namespace Jeuci.WeChatApp.Purchase.Dtos
{
    public class ServiceInfoInputBase
    {
        /// <summary>
        /// 商品描述
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal total_fee { get; set; }

        public string description { get; set; }

        public string sid { get; set; }
    }
}