using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Therapim.Filters;
using Therapim.Models;

namespace Therapim.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
//            HttpContext.Session.Remove("SessionId");
  //          HttpContext.Session.Remove("UserId");


            var testUserId = HttpContext.Session.GetString("UserId");
            _logger.LogInformation("★★★Index : セッションのユーザID　" + testUserId);

            var testSessionId = HttpContext.Session.GetString("SessionId");
            _logger.LogInformation("★★★Index : セッションのセッションID　" + testSessionId);

            _logger.LogWarning("★★★Index");
            return View();
        }

        //[PermissionAuthorize(false, "adult", "silver", "age30s")] // いずれかの権限タイプが必要
        //[PermissionAuthorize(true, "adult", "silver", "age30s")] // 全ての権限タイプが必要
        //[RankAuthorize(3)] // 権限ランク3以上が必要
        public IActionResult Privacy()
        {

            var testUserId = HttpContext.Session.GetString("UserId");
            _logger.LogInformation("★★★XtLogin : 格納して取得したユーザID　" + testUserId);

            var testSessionId = HttpContext.Session.GetString("SessionId");
            _logger.LogInformation("★★★XtLogin : 格納して取得したセッションID　" + testSessionId);

            _logger.LogWarning("★★★Privacy");
            return View();
        }


        public IActionResult Error()
        {
            // TempData からエラーメッセージを取得
            var errorMessage = TempData["ErrorMessage"] as string;

            // エラーメッセージが存在する場合、ビューに渡す
            return View(new ErrorViewModel { Message = errorMessage });
        }

        /*
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        */
    }
}
