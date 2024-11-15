using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text;
using Therapim.Models;
using Therapim.Const;
using Therapim.Processers;
using Therapim.Services;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Therapim.Filters;

namespace Therapim.Controllers
{
    /// <summary>
    /// メニュー用コントローラークラス
    /// </summary>
    public class CourseController : Controller
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly ICommonService _commonService;

        //DIコンテナより取得した機能のインスタンスを初期化しておく
        public CourseController(
            ILogger<ReviewController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        /// <summary>
        /// メニュー一覧画面表示
        /// </summary>
        /// <returns>メニュー一覧画面 with <CourseResponseModel></returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            //取得処理
            var CourseProcesser = new CourseProccesser(_logger, _commonService);
            var CourseList = await CourseProcesser.getCourseList();

            if(CourseList == null)
            {
                // TempData にエラーメッセージを設定
                TempData["ErrorMessage"] = "メニュー情報取得の通信に失敗しました。再度お試しいただいても解決しない場合は店舗までお問い合わせください。";
                // エラー画面にリダイレクト
                return RedirectToAction("Error", "Home");
            }
            //一覧画面で、来店回数と名前を表示
            ViewData["VisitedTimes"] = _commonService.GetSessionVisitedTimes();
            ViewData["FullName"] = _commonService.GetCookieFullName();
            ViewData["WelcomeMessage"] = "あと1回ご来店いただくとポイントがたまります。";

            return View(CourseList);
            //return View();
        }
    }
}
