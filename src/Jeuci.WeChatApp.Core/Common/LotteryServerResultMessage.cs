namespace Jeuci.WeChatApp.Common
{
    public class LotteryServerResultMessage<T>
        where T: class 
    {
        public string Code { get; set; }

        public string Msg { get; set; }

        public string MsgType { get; set; }

        public T Data { get; set; }
    }
}