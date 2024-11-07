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
        string GetCookieMailAddress();
        HttpClient HttpClientFactory { get; }
        IHttpContextAccessor HttpContextAccessor { get; }
    }

}
