namespace Therapim.Services
{

    public interface ICommonService
    {
        string GetSessionUserId();
        string GetSessionSessionId();
        string GetCookieUserId();
        string GetCookieSessionId();
        int GetSessionVisitedTimes();
        string GetCookieFullName();
        string GetCookieBirthday();
        string GetCookiePhoneNumber();
        HttpClient HttpClientFactory { get; }
        IHttpContextAccessor HttpContextAccessor { get; }
    }

}
