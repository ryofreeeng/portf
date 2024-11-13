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
    /// レビュー用プロセッサークラス
    /// </summary>
    public class ReviewProccesser
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly ICommonService _commonService;

        //DIコンテナより取得した機能のインスタンスを初期化しておく
        public ReviewProccesser(
            ILogger<ReviewController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        /// <summary>
        /// Gas APIよりレビュー一覧を取得する
        /// </summary>
        /// <returns>レビュー一覧 List<ReviewResponseModel></returns>
        public async Task<ReviewResponseModel> getReviewList()
        {
            //リクエスト文字列を作成
            var requestJson = @"{""targetFunc"":""review"", ""crudType"": ""read"", ""targetRange"": ""all"", ""sqlCond"": """", ""input"": """"}";
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
            var responseObject = JsonConvert.DeserializeObject<ReviewResponseModel>(responseString);

            // レスポンスデータを加工する　加工方法未定のため保留
            //var responseObjectVH = ProcessResponseData(responseObject);
            var responseObjectVH = responseObject;

            return responseObjectVH;
        }


        /// <summary>
        /// Gas APIよりレビュー情報を取得する（ReviewRequestModelに変換）
        /// ReviewResponseModelに格納する処理と、画面用にReviewRequestModelに変換する処理は別メソッドにした
        /// </summary>
        /// <returns>レビュー情報 <ReviewResponseModel></returns>
        public async Task<ReviewRequestModel> getOneReviewForEdit(int reviewId)
        {
            //１レビュー情報取得メソッド呼び出し
            ReviewResponseModel getOneReviewResult = await getOneReview(reviewId);
            if (getOneReviewResult == null)
            {
                return null;
            }
            //レビュー情報部分のみを取り出す
            var oneReview = getOneReviewResult.SqlContent[0];
            //取得したレビュー情報と画面に紐づけるモデルクラスは異なるため、一度シリアライズしてからデシリアライズする
            var jsonString = JsonConvert.SerializeObject(oneReview);
            var reviewObject = JsonConvert.DeserializeObject<ReviewRequestModel>(jsonString);

            return reviewObject;
        }

        //レビュー取得処理（RerviewResponseModelに格納）
        public async Task<ReviewResponseModel> getOneReview(int reviewId)
        {
            //リクエスト文字列を作成
            ReviewRequestModel model = new ReviewRequestModel();
            model.ReviewId = reviewId;
            model.UserId = _commonService.GetSessionUserId();

            var conditionJson = new
            {
                where = new { ReviewId = model.ReviewId, UserId = model.UserId }
            };

            string jsonConditionString = JsonConvert.SerializeObject(conditionJson, Formatting.Indented);
            var requestJson = @"{""targetFunc"":""review"", ""crudType"": ""read"", ""targetRange"": ""one"", ""sqlCond"": "
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
            var responseObject = JsonConvert.DeserializeObject<ReviewResponseModel>(responseString);

            return responseObject;
        }


        /// <summary>
        /// Gas APIをたたいてレビューを登録する
        /// </summary>
        /// <returns>レビュー登録結果 <ReviewResponseModel></returns>
        public async Task<ReviewResponseModel> createOneReview(ReviewRequestModel model)
        {
            //セッションIDはここでAPIへのリクエスト用に追加する
            model.CreatedSessionId = _commonService.GetSessionSessionId();

            string jsonContentString = JsonConvert.SerializeObject(model, Formatting.Indented);
            var requestJson = @"{""targetFunc"":""review"", ""crudType"": ""create"", ""targetRange"": ""one"", ""sqlCond"": """",""input"": "
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
            var responseObject = JsonConvert.DeserializeObject<ReviewResponseModel>(responseString);

            return responseObject;
        }


        /// <summary>
        /// Gas APIをたたいてレビューを更新する
        /// </summary>
        /// <returns>レビュー更新結果 <ReviewResponseModel></returns>
        public async Task<ReviewResponseModel> updateOneReview(ReviewRequestModel model)
        {
            //リクエスト文字列を作成            
            model.UserId = _commonService.GetSessionUserId();
            var conditionJson = new
            {
                where = new { ReviewId = model.ReviewId, UserId = model.UserId }
            };

            string jsonConditionString = JsonConvert.SerializeObject(conditionJson, Formatting.Indented);

            model.UpdatedSessionId = _commonService.GetSessionSessionId();
            string jsonContentString = JsonConvert.SerializeObject(model, Formatting.Indented); 
            
            var requestJson = @"{""targetFunc"":""review"", ""crudType"": ""update"", ""targetRange"": ""one"", ""sqlCond"": "
                                + jsonConditionString
                                + @",""input"": "
                                + jsonContentString
                                + @"}";            
            
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
            var responseObject = JsonConvert.DeserializeObject<ReviewResponseModel>(responseString);

            return responseObject;
        }
    }
}
