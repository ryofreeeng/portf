namespace Therapim.Services
{

    public interface ICommonService
    {
        string GetSessionUserId();
        string GetSessionSessionId();
        string GetCookieUserId();
        string GetCookieSessionId();
        HttpClient HttpClientFactory { get; }
        IHttpContextAccessor HttpContextAccessor { get; }
    }

}
