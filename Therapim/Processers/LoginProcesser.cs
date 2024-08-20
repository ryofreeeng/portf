using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text;
using Therapim.Models;
using Therapim.Const;
using System.Net.Http;
using Therapim.Controllers;
using Therapim.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Therapim.Processers
{
    /// <summary>
    /// レビュー用プロセッサークラス
    /// </summary>
    public class LoginProccesser
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ICommonService _commonService;

        //DIコンテナより取得した機能のインスタンスを初期化しておく
        public LoginProccesser(
            ILogger<LoginController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        /// <summary>
        /// Gas APIより顧客情報を取得する
        /// </summary>
        /// <returns>顧客情報　<LoginResponseModel> </returns>
        public async Task<LoginResponseModel> getOneCustomer(LoginRequestModel model)
        {
            //SQL用の文字列を作成
            var conditionJson = new
            {
                where = new
                {
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Birthday = model.Birthday
                }
            };

            string jsonConditionString = JsonConvert.SerializeObject(conditionJson, Formatting.Indented);
            var requestJson = @"{""targetFunc"":""customer"", ""crudType"": ""read"", ""targetRange"": ""one"", ""sqlCond"": "
                                + jsonConditionString
                                + @",""input"": """"}";
            var request = new StringContent(requestJson, Encoding.UTF8, "application/json");

            //リクエストを送信してレスポンス取得
            var response = await _commonService.HttpClientFactory.PostAsync(ConstClass.GAS_API_URL, request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            //たぶんいらないこれ
            //var responseString = await response.Content.ReadAsStringAsync();
            //var responseObject = JsonConvert.DeserializeObject<LoginResponseModel>(responseString);

            var users = await response.Content.ReadFromJsonAsync<LoginResponseModel>();

            return users;
        }


        /// <summary>
        /// ログイン処理を行う
        /// </summary>
        /// <returns>なし</returns>
        public async Task<bool> executeLogin(LoginResponseModel users)
        {
            //結果返却用変数
            bool loginResult = false;

            //顧客情報が取得できた場合のみログイン処理
            if (users.SqlContent.Count != 0)
            {
                var user = users.SqlContent[0];
                _logger.LogInformation($"★★★APIから取得したユーザID：{user.UserId}");

                //セッションID生成
                var sessionId = "session_" + user.UserId + "_" + DateTime.Now;

                // クッキーに格納するクレーム情報リストを作成。Permissionは後から追加処理をするため、配列ではなくサイズ変更可能なListで作成する
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId),
                    new Claim("SessionId",sessionId),
                    new Claim(ClaimTypes.Name,user.FullName),
                    new Claim("Birthday",user.Birthday.ToString()),
                    new Claim("PhoneNumber",user.PhoneNumber),
                };
                //Permissionを一つずつ取り出してクレーム情報リストに追加
                if (!string.IsNullOrEmpty(user.Permission))
                {
                    claims.AddRange(
                        user.Permission.Split(',')
                                        .Select(permission => new Claim("Permission", permission))
                    );
                }

                //クッキー認証を指定して情報を設定
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // 永続的なクッキーにするかどうか（ブラウザを閉じても残るかどうか）
                    ExpiresUtc = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo")).AddHours(24) // クッキーの有効期限を1日に設定。大元の設定もあるがこちらで上書きしている状態
                };

                // ユーザーをサインイン
                await _commonService.HttpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                // セッションにユーザー情報を保存
                _commonService.HttpContextAccessor.HttpContext.Session.SetString("UserId", user.UserId);
                _commonService.HttpContextAccessor.HttpContext.Session.SetString("SessionId", sessionId);
                _commonService.HttpContextAccessor.HttpContext.Session.SetString("FullName", user.FullName);
                _commonService.HttpContextAccessor.HttpContext.Session.SetString("Birthday", user.Birthday.ToString());
                _commonService.HttpContextAccessor.HttpContext.Session.SetString("PhoneNumber", user.PhoneNumber);
                _commonService.HttpContextAccessor.HttpContext.Session.SetString("Rank", user.Rank.ToString());

                loginResult = true;
                return loginResult;
            }
            return loginResult;
        }

    }
}
