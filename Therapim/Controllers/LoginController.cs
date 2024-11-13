﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text;
using Therapim.Models;
using Therapim.Const;
using Therapim.Processers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Xml.Linq;
using Therapim.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;

namespace Therapim.Controllers
{
    /// <summary>
    /// ログイン関連処理用コントローラークラス
    /// </summary>
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ICommonService _commonService;

        //DIコンテナより取得した機能のインスタンスを初期化しておく
        public LoginController(
            ILogger<LoginController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        /// <summary>
        /// ログイン画面表示
        /// </summary>
        /// <returns>ログイン画面</returns>
        [HttpGet]
        public IActionResult Index()
        {
            //開始ログ出力
            _logger.LogInformation($"★{ControllerContext.ActionDescriptor.ControllerName}/{ControllerContext.ActionDescriptor.ActionName}　開始");

            //セッション・クッキー情報のログ出力         
            _logger.LogInformation($"★★SessionIdInSession : {_commonService.GetSessionSessionId()}");
            _logger.LogInformation($"★★SessionIdInCookie : {_commonService.GetCookieSessionId()}");
            _logger.LogInformation($"★★UserIdInSession : {_commonService.GetSessionUserId()}");
            _logger.LogInformation($"★★UserIdInCookie : {_commonService.GetCookieUserId()}");

            //クッキーにユーザIDが存在する場合はTOPへリダイレクト
            if (_commonService.GetCookieUserId() != null)
            {
                _logger.LogInformation($"★★★userIdInCookieが存在するためログインの必要はありません");
                _logger.LogInformation($"★{ControllerContext.ActionDescriptor.ControllerName}/{ControllerContext.ActionDescriptor.ActionName}　終了");
                return RedirectToAction("Index", "Home");
            }

            _logger.LogWarning("★★★userIdInSession == nullのためログイン画面へ遷移します");

            // リクエスト元のURLを取得
            var referrer = Request.Headers["Referer"].ToString();

            // リファラーが空でない場合は、リファラーをViewBagに格納
            ViewData["ReturnUrl"] = !string.IsNullOrEmpty(referrer) ? referrer : Url.Action("Index", "Home");


            _logger.LogInformation($"★{ControllerContext.ActionDescriptor.ControllerName}/{ControllerContext.ActionDescriptor.ActionName}　終了");
            return View();
        }

        /// <summary>
        /// ログイン処理
        /// </summary>
        /// <param name="model">ログイン用リクエスト情報</  >
        /// <returns>ログイン成功時：ホーム画面、ログイン失敗時：ログイン画面</returns>
        [HttpPost]
        public async Task<IActionResult> XtLogin(LoginRequestModel model)
        {
            //開始ログ出力
            _logger.LogInformation($"★{ControllerContext.ActionDescriptor.ControllerName}/{ControllerContext.ActionDescriptor.ActionName}　開始");

            // バリデーションエラーがある場合は入力画面を再表示
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            //顧客情報取得
            LoginProccesser loginProccesser = new LoginProccesser(_logger, _commonService);
            LoginResponseModel users = await loginProccesser.getOneCustomer(model);

            //レスポンスが空の場合はエラー画面へ遷移
            if (users == null)
            {
                // TempData にエラーメッセージを設定
                TempData["ErrorMessage"] = "ログイン情報取得の通信に失敗しました。再度お試しいただいても解決しない場合は店舗までお問い合わせください。";
                // エラー画面にリダイレクト
                return RedirectToAction("Error", "Home");
            }

            //ログイン実行
            if(await loginProccesser.executeLogin(users))
            {
                _logger.LogInformation($"★リダイレクトURLは「{model.ReturnUrl}」");
                //ログイン処理成功後はホーム画面へ ▲あとでリダイレクト機能も追加
                return Redirect(model.ReturnUrl ?? Url.Action("Index", "Home"));
            }
            else 
            {
                //顧客情報が0件だった場合
                ModelState.AddModelError(string.Empty, "ログイン情報が見つかりませんでした。入力情報が不明な場合はお問い合わせください☆");
                ViewData["ReturnUrl"] = !string.IsNullOrEmpty(model.ReturnUrl) ? model.ReturnUrl : Url.Action("Index", "Home");
                return View("Index", model);
            }                
        }            
    
            
            
        

        /// <summary>
        /// ログアウト処理
        /// </summary>
        /// <returns>ログイン画面へリダイレクト</returns>
        /*
        [HttpPost]
        public IActionResult XtLogout()
        {
            //セッション情報削除
            HttpContext.Session.Clear();
            //クッキーからセッションID削除
            HttpContext.Response.Cookies.Delete("SessionId");
            return RedirectToAction("Index","Login");
        }
        */


        [HttpGet]
        public async Task<IActionResult> XtLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear(); // セッション情報をクリア
            return RedirectToAction("Index", "Login");
        }


    }
}
