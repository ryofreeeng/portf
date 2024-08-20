using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;


namespace Therapim.Filters
{
    /// <summary>
    /// ログイン済かつ権限確認用フィルター　特定の権限を持つかチェックする
    /// </summary>
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] _permissions;
        private readonly bool _allRequired;

        public PermissionAuthorizeAttribute(bool allRequired, params string[] permissions)
        {
            _permissions = permissions;
            _allRequired = allRequired;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userPermissions = user.Claims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();

            if (_allRequired)
            {
                // 全ての権限タイプが必要
                if (!_permissions.All(p => userPermissions.Contains(p)))
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                // いずれかの権限タイプが必要
                if (!_permissions.Any(p => userPermissions.Contains(p)))
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }

}
