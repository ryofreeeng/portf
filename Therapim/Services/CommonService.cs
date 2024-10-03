using System.Security.Claims;

namespace Therapim.Services
{
    public class CommonService : ICommonService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public CommonService(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public string GetSessionUserId() => _httpContextAccessor.HttpContext.Session.GetString("UserId");

        public string GetSessionSessionId() => _httpContextAccessor.HttpContext.Session.GetString("SessionId");

        public string GetCookieUserId() => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string GetCookieSessionId() => _httpContextAccessor.HttpContext.User.FindFirst("SessionId")?.Value;

        public int GetSessionVisitedTimes() => !int.TryParse(_httpContextAccessor.HttpContext.User.FindFirst("VisitedTimes")?.Value,out int result)? 0: int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("VisitedTimes")?.Value);
        public HttpClient HttpClientFactory => _httpClientFactory.CreateClient();

        public IHttpContextAccessor HttpContextAccessor => _httpContextAccessor;
    }
}