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
    /// レビュー用コントローラークラス
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
        /// コース一覧画面表示
        /// </summary>
        /// <returns>コース一覧画面 with <CourseResponseModel></returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            //取得処理
            //var CourseProcesser = new CourseProccesser(_logger, _commonService);
            //var CourseList = await CourseProcesser.getCourseList();

            //if(CourseList == null)
            //{
                // TempData にエラーメッセージを設定
                //TempData["ErrorMessage"] = "コース情報取得の通信に失敗しました。再度お試しいただいても解決しない場合は店舗までお問い合わせください。";
                // エラー画面にリダイレクト
                //return RedirectToAction("Error", "Home");
            //}
            //一覧画面で、ユーザIDが一致するレビューには編集ボタンを表示するためのパラメータを持たせる
            ViewData["VisitedTimes"] = 3;
            ViewData["WelcomeMessage"] = "あと1回ご来店いただくとポイントがたまります。";

            //return View(CourseList);
            return View();
        }


        /// <summary>
        /// レビュー登録画面
        /// </summary>
        /// <returns>レビュー登録画面</returns>
        [TypeFilter(typeof(LoginAuthorizeAttribute))]
        [HttpGet]
        public IActionResult Create()
        {
            // 新規作成の場合は空のモデルを返す
            var review = new ReviewRequestModel();
            return View("CreateOrEdit",review);
        }

        /// <summary>
        /// レビュー登録処理
        /// </summary>
        /// <returns>レビュー登録処理後の遷移画面</returns>
        [TypeFilter(typeof(LoginAuthorizeAttribute))]
        [HttpPost]
        public async Task<IActionResult> Create(ReviewRequestModel model)
        {
            //userIdは画面になく、nullになっているためセッションのuserIdで上書き
            var userId = _commonService.GetSessionUserId();
            if (string.IsNullOrEmpty(userId))
            {
                // エラーハンドリング
                ModelState.AddModelError(string.Empty, "ログイン情報が古くなっています。ログインしなおしてください");
                return View("CreateOrEdit", model);
            }
            model.UserId = userId;

            // 再度バリデーションを実行してModelStateを更新する
            TryValidateModel(model);
            // バリデーションエラーがある場合は新規登録画面を再表示
            if (!ModelState.IsValid)
            {
                return View("CreateOrEdit", model);
            }

            // 登録処理
            var reviewProcesser = new ReviewProccesser(_logger, _commonService);
            var createReviewResult = await reviewProcesser.createOneReview(model);

            if (createReviewResult == null)
            {
                // TempData にエラーメッセージを設定
                TempData["ErrorMessage"] = "レビュー登録の通信に失敗しました。再度お試しいただいても解決しない場合は店舗までお問い合わせください。";
                // エラー画面にリダイレクト
                return RedirectToAction("Error", "Home");
            }

            // 成功時は一覧画面へリダイレクト
            return RedirectToAction("List");
        }



        /// <summary>
        /// レビュー編集画面
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns>レビュー編集画面</returns>
        [TypeFilter(typeof(LoginAuthorizeAttribute))]
        [HttpGet]
        public async Task<IActionResult> Edit(int reviewId)
        {
            // レビューIDが存在しない場合は新規登録にリダイレクト
            if (reviewId <= 0)
            {                
                return RedirectToAction("Create");
            }

            // 編集対象のレビュー情報取得処理
            var reviewProcesser = new ReviewProccesser(_logger, _commonService);
            var review = await reviewProcesser.getOneReviewForEdit(reviewId);

            if(review == null)
            {
                // TempData にエラーメッセージを設定
                TempData["ErrorMessage"] = "編集するレビュー情報取得の通信に失敗しました。再度お試しいただいても解決しない場合は店舗までお問い合わせください。";
                // エラー画面にリダイレクト
                return RedirectToAction("Error", "Home");
            }
            //取得されたレビューのユーザIDが現在のログイン情報と異なっていたらエラー
            if (review.UserId != _commonService.GetSessionUserId()) {
                // TempData にエラーメッセージを設定
                TempData["ErrorMessage"] = "現在のログイン情報で作成されたレビューではないため、ログインしなおしてください。または店舗までお問い合わせ下さい。";
                // エラー画面にリダイレクト
                return RedirectToAction("Error", "Home");
            }

            return View("CreateOrEdit",review);
        }


        /// <summary>
        /// レビュー更新処理
        /// </summary>
        /// <returns>レビュー更新処理後の遷移画面</returns>
        [TypeFilter(typeof(LoginAuthorizeAttribute))]
        [HttpPost]
        public async Task<IActionResult> Edit(ReviewRequestModel model)
        {
            //userIdは画面になく、nullになっているためセッションのuserIdで上書き
            var userId = _commonService.GetSessionUserId();
            if (string.IsNullOrEmpty(userId))
            {
                // エラーハンドリング
                ModelState.AddModelError(string.Empty, "ログイン情報が古くなっています。ログインしなおしてください");
                return View("CreateOrEdit", model);
            }
            model.UserId = userId;

            // 再度バリデーションを実行してModelStateを更新する
            TryValidateModel(model);
            // バリデーションエラーがある場合は編集画面を再表示
            if (!ModelState.IsValid)
            {
                return View("CreateOrEdit", model);
            }

            // 更新処理
            var reviewProcesser = new ReviewProccesser(_logger, _commonService);
            var updateReviewResult = await reviewProcesser.updateOneReview(model);

            if (updateReviewResult == null)
            {
                // TempData にエラーメッセージを設定
                TempData["ErrorMessage"] = "レビュー情報更新の通信に失敗しました。再度お試しいただいても解決しない場合は店舗までお問い合わせください。";
                // エラー画面にリダイレクト
                return RedirectToAction("Error", "Home");
            }

            // 成功時は一覧画面へリダイレクト
            return RedirectToAction("List");
        }


}
}
