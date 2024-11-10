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
    /// 予約用プロセッサークラス
    /// </summary>
    public class ReservationProcesser
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly ICommonService _commonService;

        //DIコンテナより取得した機能のインスタンスを初期化しておく
        public ReservationProcesser(
            ILogger<ReviewController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        /// <summary>
        /// Gas APIをたたいて予約を登録する
        /// </summary>
        /// <returns>予約登録結果 <ReviewResponseModel></returns>
        public async Task<ReservationResponseModel> createOneReservation(ReservationRequestModel model)
        {
            //セッションIDはここでAPIへのリクエスト用に追加する
            model.CreatedSessionId = _commonService.GetSessionSessionId();

            string jsonContentString = JsonConvert.SerializeObject(model, Formatting.Indented);
            var requestJson = @"{""targetFunc"":""reservation"", ""crudType"": ""create"", ""targetRange"": ""one"", ""sqlCond"": """",""input"": "
                                + jsonContentString + @"}";
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
            var responseObject = JsonConvert.DeserializeObject<ReservationResponseModel>(responseString);

            return responseObject;
        }


    }
}
