namespace Jeuci.WeChatApp.Policy
{
    public interface IApiAuthorizePolicy
    {
        bool IsValidTime();

        bool IsLegalSign();
    }
}