namespace Therapim.Services
{

    public interface ICommonService
    {
        string GetSessionUserId();
        string GetSessionSessionId();
        string GetCookieUserId();
        string GetCookieSessionId();
        int GetSessionVisitedTimes();
        HttpClient HttpClientFactory { get; }
        IHttpContextAccessor HttpContextAccessor { get; }
    }

}
