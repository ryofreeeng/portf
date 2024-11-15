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

namespace Therapim.Processers
{
    /// <summary>
    /// メニュー用プロセッサークラス
    /// </summary>
    public class CourseProccesser
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly ICommonService _commonService;

        //DIコンテナより取得した機能のインスタンスを初期化しておく
        public CourseProccesser(
            ILogger<ReviewController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        /// <summary>
        /// Gas APIよりメニュー一覧を取得する
        /// </summary>
        /// <returns>メニュー一覧 List<CourseResponseModel></returns>
        public async Task<CourseResponseModel> getCourseList()
        {
            //リクエスト文字列を作成
            CourseRequestModel model = new CourseRequestModel();
            model.UserId = _commonService.GetCookieUserId();

            var conditionJson = new
            {
                where = new { UserId = model.UserId }
            };

            string jsonConditionString = JsonConvert.SerializeObject(conditionJson, Formatting.Indented);
            var requestJson = @"{""targetFunc"":""course"", ""crudType"": ""read"", ""targetRange"": ""all"", ""sqlCond"": "
                                + jsonConditionString
                                + @",""input"": """"}";
            var request = new StringContent(requestJson, Encoding.UTF8, "application/json");

            //リクエストを送信してレスポンス取得
            var response = await _commonService.HttpClientFactory.PostAsync(ConstClass.GAS_API_URL, request);
            //レスポンスが取得できなければnullを返す
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            //レスポンスをオブジェクトに加工
            var responseString = await response.Content.ReadAsStringAsync();            
            var responseObject = JsonConvert.DeserializeObject<CourseResponseModel>(responseString);

            // レスポンスデータを加工する　加工方法未定のため保留
            //var responseObjectVH = ProcessResponseData(responseObject);
            var responseObjectVH = responseObject;

            return responseObjectVH;
        }


    }
}
