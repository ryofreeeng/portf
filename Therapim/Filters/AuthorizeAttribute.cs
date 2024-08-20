/*
 * using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Therapim.Controllers;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Therapim.Filters
{
    /// <summary>
    /// ログイン状態を確認するフィルター
    /// [Authorize]を各アクションメソッドに付与して使用する
    /// </summary>

    //AuthorizeAtributeクラスの宣言でTypeFilterAttributeを実装することで、このクラスが属性として使用できる
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        //base(=TypeFilterAttribute)のコンストラクタでは、フィルターのタイプを指定する
        public AuthorizeAttribute() : base(typeof(AuthorizeFilter))
        {
        }

        //フィルターの定義
        private class AuthorizeFilter : IAuthorizationFilter
        {
            //コントローラークラスではHttpContextでアクセスできるが、他クラスではできないためIHttpContextAccessorを使用する
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILogger<AuthorizeFilter> _logger;

            public AuthorizeFilter(IHttpContextAccessor httpContextAccessor, ILogger<AuthorizeFilter> logger)
            {
                _httpContextAccessor = httpContextAccessor;
                _logger = logger;
            }

            //このフィルターでは「OnAuthorization」という名称のメソッドがチェックロジックとして実行される
            //このチェック処理は自由にカスタマイズ可能。AuthorizationFilterContextは、チェック対象のアクションメソッドのコンテキスト情報
            public void OnAuthorization(AuthorizationFilterContext context)
            {


                var builder = WebApplication.CreateBuilder();
                //var logger = builder.Logging.Services.AddLogging;

                //クッキーのセッションIDをもとに、セッションにUserIdがあるかを確認し、なければリダイレクトする
                //ログイン後に戻ってくるページURLを渡す処理を後で追加したい▲
                var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                _logger.LogWarning("★★★"+userId+ "★★★");

                if (userId == null)
                {
                    _logger.LogWarning("★★★フィルターのリダイレクト前");

                    context.Result = new RedirectToActionResult("Index", "Home", null);
                }
            }
        }
    }

}
*/