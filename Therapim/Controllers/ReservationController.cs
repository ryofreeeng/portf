using Microsoft.AspNetCore.Mvc;
using Therapim.Models;
using Therapim.Processers;
using Therapim.Services;

namespace Therapim.Controllers
{
    /// <summary>
    /// 予約用コントローラークラス
    /// </summary>
    public class ReservationController : Controller
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly ICommonService _commonService;

        //DIコンテナより取得した機能のインスタンスを初期化しておく
        public ReservationController(
            ILogger<ReviewController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }


        /// <summary>
        /// 予約登録画面
        /// </summary>
        /// <returns>予約登録画面</returns>        
        [HttpGet]
        public async Task<IActionResult> Create(string? MenuId)
        {
            try
            {
                // 新規作成の場合は空のモデルを返すので作成
                var reservation = new ReservationRequestModel();

                // ログイン中の場合は個人情報を格納しておく
                reservation.FullName = _commonService.GetCookieFullName();
                reservation.Birthday = _commonService.GetCookieBirthday();
                reservation.PhoneNumber = _commonService.GetCookiePhoneNumber();
                reservation.MailAddress = _commonService.GetCookieMailAddress();

                // メニュー情報取得処理
                var CourseProcesser = new CourseProccesser(_logger, _commonService);
                var CourseList = await CourseProcesser.getCourseList();

                if (CourseList == null)
                {
                    // TempData にエラーメッセージを設定
                    TempData["ErrorMessage"] = "メニュー情報取得の通信に失敗しました。再度お試しいただいても解決しない場合は店舗までお問い合わせください。";
                    // エラー画面にリダイレクト
                    return RedirectToAction("Error", "Home");
                }

                ViewData["CourseList"] = CourseList;

                // メニュー画面から来た場合はパラメータのメニューID取得して渡す
                ViewData["MenuId"] = MenuId?.ToString() ?? string.Empty;

                return View("Create", reservation);
            }
            catch (Exception ex)
            {
                // TempData にエラーメッセージを設定
                TempData["ErrorMessage"] = "予約画面の取得に失敗しました。再度お試しいただいても解決しない場合は店舗までお問い合わせください。";
                // エラー画面にリダイレクト
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        /// 予約登録画面(POSTBACK時)
        /// </summary>
        /// <returns>予約登録画面(POSTBACK時)</returns>        
        [HttpPost]
        public async Task<IActionResult> Create(ReservationRequestModel model)
        {
            try
            {
                // メニュー情報取得処理
                var CourseProcesser = new CourseProccesser(_logger, _commonService);
                var CourseList = await CourseProcesser.getCourseList();

                if (CourseList == null)
                {
                    // TempData にエラーメッセージを設定
                    TempData["ErrorMessage"] = "戻る際にメニュー情報取得の通信に失敗しました。再度お試しいただいても解決しない場合は店舗までお問い合わせください。";
                    // エラー画面にリダイレクト
                    return RedirectToAction("Error", "Home");
                }

                ViewData["CourseList"] = CourseList;

                return View("Create", model);
            }
            catch (Exception ex)
            {
                // TempData にエラーメッセージを設定
                TempData["ErrorMessage"] = "予約画面の再取得に失敗しました。再度お試しいただいても解決しない場合は店舗までお問い合わせください。";
                // エラー画面にリダイレクト
                return RedirectToAction("Error", "Home");
            }        
        }


        /// <summary>
        /// 予約登録確認画面
        /// </summary>
        /// <returns>予約登録確認画面</returns>        
        [HttpPost]
        public async Task<IActionResult> CreateConfirm(ReservationRequestModel model)
        {
            try
            {
                //userIdは画面になく、nullになっているためセッションのuserIdで上書き(必須ではないのでnullでもよい)
                model.UserId = _commonService.GetSessionUserId();

                // 再度バリデーションを実行してModelStateを更新する
                TryValidateModel(model);
                // バリデーションエラーがある場合は入力画面を再表示
                if (!ModelState.IsValid)
                {
                    // メニュー情報取得処理
                    var CourseProcesser = new CourseProccesser(_logger, _commonService);
                    var CourseList = await CourseProcesser.getCourseList();

                    if (CourseList == null)
                    {
                        // TempData にエラーメッセージを設定
                        TempData["ErrorMessage"] = "メニュー情報取得の通信に失敗しました。再度お試しいただいても解決しない場合は店舗までお問い合わせください。";
                        // エラー画面にリダイレクト
                        return RedirectToAction("Error", "Home");
                    }

                    ViewData["CourseList"] = CourseList;
                    return View("Create", model);
                }

                // 問題なければ確認画面へ遷移
                return View("CreateConfirm", model);
            }
            catch (Exception ex)
            {
                // TempData にエラーメッセージを設定
                TempData["ErrorMessage"] = "予約確認画面の取得に失敗しました。再度お試しいただいても解決しない場合は店舗までお問い合わせください。";
                // エラー画面にリダイレクト
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// 予約登録確認画面
        /// </summary>
        /// <returns>予約登録確認画面</returns>        
        [HttpGet]
        public async Task<IActionResult> CreateConfirm()
        {
            // TempData にエラーメッセージを設定
            TempData["ErrorMessage"] = "ログイン後に入力中の画面に戻ることはできません。お手数ですが入力を再度お願いいたします。";
            // エラー画面にリダイレクト
            return RedirectToAction("Error", "Home");
        }
            /// <summary>
            /// 予約登録処理
            /// </summary>
            /// <returns>予約登録処理後の遷移画面</returns>
            [HttpPost]
        public async Task<IActionResult> XtCreate(ReservationRequestModel model)
        {
            try
            {
                //userIdは画面になく、nullになっているためセッションのuserIdで上書き(必須ではないのでnullでもよい)
                model.UserId = _commonService.GetSessionUserId();

                // セッションのユーザIDがない場合は、クッキーのユーザIDを格納しておく
                if(String.IsNullOrEmpty(model.UserId))
                {
                    model.UserId = _commonService.GetCookieUserId();
                }

                // 再度バリデーションを実行してModelStateを更新する
                TryValidateModel(model);
                // バリデーションエラーがある場合は確認画面を再表示
                if (!ModelState.IsValid)
                {
                    return View("CreateConfirm", model);
                }

                // 登録処理
                var reservationProcesser = new ReservationProcesser(_logger, _commonService);
                var createReservationResult = await reservationProcesser.createOneReservation(model);

                if (createReservationResult == null)
                {
                    // TempData にエラーメッセージを設定
                    TempData["ErrorMessage"] = "予約申込の通信に失敗しました。再度お試しいただいても解決しない場合は店舗までお問い合わせください。";
                    // エラー画面にリダイレクト
                    return RedirectToAction("Error", "Home");
                }

                // 成功時は完了画面を表示
                return View("CreateComplete", model);
            }
            catch (Exception ex)
            {
                // TempData にエラーメッセージを設定
                TempData["ErrorMessage"] = "仮予約登録の処理に失敗しました。再度お試しいただいても解決しない場合は店舗までお問い合わせください。";
                // エラー画面にリダイレクト
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
