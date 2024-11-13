using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Therapim.Filters
{
    /// <summary>
    /// ログイン済かつランク確認用フィルター　一定以上のランクを持つかチェックする
    /// </summary>
    public class RankAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly int _minRank;

        public RankAuthorizeAttribute(int minRank)
        {
            _minRank = minRank;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var rankClaim = user.Claims.FirstOrDefault(c => c.Type == "Rank");
            if (rankClaim == null || !int.TryParse(rankClaim.Value, out var userRank) || userRank < _minRank)
            {
                //これは単に文字列返すだけ
                context.Result = new ContentResult
                {
                    Content = "You do not have permission to view this page.",
                    ContentType = "text/plain",
                    StatusCode = StatusCodes.Status403Forbidden
                };

                //context.Result = new RedirectToActionResult("Index", "Home", null);　こちらで特定のアクションへリダイレクトする
            }
        }
    }


}
