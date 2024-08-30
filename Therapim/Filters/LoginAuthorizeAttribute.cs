/* program.csの認証処理の方で同じ宛先へリダイレクトされるのでこちらは不要だった
　もし分岐させたいならそちらで処理を記述する
*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Therapim.Controllers;
using Therapim.Services;

namespace Therapim.Filters
{
    /// <summary>
    /// ログイン済確認用フィルター　未ログイン時はログイン画面へリダイレクトする
    /// </summary>
    public class LoginAuthorizeAttribute : IAuthorizationFilter
    {
        private readonly ILogger<LoginAuthorizeAttribute> _logger;
        private readonly ICommonService _commonService;

        public LoginAuthorizeAttribute(ILogger<LoginAuthorizeAttribute> logger, ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            //クッキーとセッションを同じ期限にするか、クッキーが残っていればログイン状態復活するようにセッションに再度格納するか
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);　//こちらで特定のアクションへリダイレクトする
            }

            //var logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<LoginAuthorizeAttribute>)) as ILogger;            
            _logger.LogInformation("★★★★★★★★★★★ログインフィルター処理通過確認");
            
            //CookieにあるUserIdとSessionIdをセッションにも保存してログイン状態を復活させる。ただし名前などの情報は保存していないので
            //これらがセッション情報に必要なら再度ログインが必要
            _commonService.HttpContextAccessor.HttpContext.Session.SetString("UserId", _commonService.GetCookieUserId() ?? "");
            _commonService.HttpContextAccessor.HttpContext.Session.SetString("SessionId", _commonService.GetCookieSessionId() ?? "");

        }
    }
}
