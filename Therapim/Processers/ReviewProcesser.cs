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
    /// ���r���[�p�v���Z�b�T�[�N���X
    /// </summary>
    public class ReviewProccesser
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly ICommonService _commonService;

        //DI�R���e�i���擾�����@�\�̃C���X�^���X�����������Ă���
        public ReviewProccesser(
            ILogger<ReviewController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        /// <summary>
        /// Gas API��背�r���[�ꗗ���擾����
        /// </summary>
        /// <returns>���r���[�ꗗ List<ReviewResponseModel></returns>
        public async Task<ReviewResponseModel> getReviewList()
        {
            //���N�G�X�g��������쐬
            var requestJson = @"{""targetFunc"":""review"", ""crudType"": ""read"", ""targetRange"": ""all"", ""sqlCond"": """", ""input"": """"}";
            var request = new StringContent(requestJson, Encoding.UTF8, "application/json");

            //���N�G�X�g�𑗐M���ă��X�|���X�擾
            var response = await _commonService.HttpClientFactory.PostAsync(ConstClass.GAS_API_URL, request);
            //���X�|���X���擾�ł��Ȃ����null��Ԃ�
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            //���X�|���X���I�u�W�F�N�g�ɉ��H
            var responseString = await response.Content.ReadAsStringAsync();            
            var responseObject = JsonConvert.DeserializeObject<ReviewResponseModel>(responseString);

            // ���X�|���X�f�[�^�����H����@���H���@����̂��ߕۗ�
            //var responseObjectVH = ProcessResponseData(responseObject);
            var responseObjectVH = responseObject;

            return responseObjectVH;
        }


        /// <summary>
        /// Gas API��背�r���[�����擾����iReviewRequestModel�ɕϊ��j
        /// ReviewResponseModel�Ɋi�[���鏈���ƁA��ʗp��ReviewRequestModel�ɕϊ����鏈���͕ʃ��\�b�h�ɂ���
        /// </summary>
        /// <returns>���r���[��� <ReviewResponseModel></returns>
        public async Task<ReviewRequestModel> getOneReviewForEdit(int reviewId)
        {
            //�P���r���[���擾���\�b�h�Ăяo��
            ReviewResponseModel getOneReviewResult = await getOneReview(reviewId);
            if (getOneReviewResult == null)
            {
                return null;
            }
            //���r���[��񕔕��݂̂����o��
            var oneReview = getOneReviewResult.SqlContent[0];
            //�擾�������r���[���Ɖ�ʂɕR�Â��郂�f���N���X�͈قȂ邽�߁A��x�V���A���C�Y���Ă���f�V���A���C�Y����
            var jsonString = JsonConvert.SerializeObject(oneReview);
            var reviewObject = JsonConvert.DeserializeObject<ReviewRequestModel>(jsonString);

            return reviewObject;
        }

        //���r���[�擾�����iRerviewResponseModel�Ɋi�[�j
        public async Task<ReviewResponseModel> getOneReview(int reviewId)
        {
            //���N�G�X�g��������쐬
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

            //���N�G�X�g�𑗐M���ă��X�|���X�擾
            var response = await _commonService.HttpClientFactory.PostAsync(ConstClass.GAS_API_URL, request);
            //���X�|���X���擾�ł��Ȃ����null��Ԃ�
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            //���X�|���X���I�u�W�F�N�g�ɉ��H
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ReviewResponseModel>(responseString);

            return responseObject;
        }


        /// <summary>
        /// Gas API���������ă��r���[��o�^����
        /// </summary>
        /// <returns>���r���[�o�^���� <ReviewResponseModel></returns>
        public async Task<ReviewResponseModel> createOneReview(ReviewRequestModel model)
        {
            //�Z�b�V����ID�͂�����API�ւ̃��N�G�X�g�p�ɒǉ�����
            model.CreatedSessionId = _commonService.GetSessionSessionId();

            string jsonContentString = JsonConvert.SerializeObject(model, Formatting.Indented);
            var requestJson = @"{""targetFunc"":""review"", ""crudType"": ""create"", ""targetRange"": ""one"", ""sqlCond"": """",""input"": "
                                + jsonContentString + @"}";
            var request = new StringContent(requestJson, Encoding.UTF8, "application/json");            

            //���N�G�X�g�𑗐M���ă��X�|���X�擾
            var response = await _commonService.HttpClientFactory.PostAsync(ConstClass.GAS_API_URL, request);
            //���X�|���X���擾�ł��Ȃ����null��Ԃ�
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            //���X�|���X���I�u�W�F�N�g�ɉ��H
            var responseString = await response.Content.ReadAsStringAsync();            
            var responseObject = JsonConvert.DeserializeObject<ReviewResponseModel>(responseString);

            return responseObject;
        }


        /// <summary>
        /// Gas API���������ă��r���[���X�V����
        /// </summary>
        /// <returns>���r���[�X�V���� <ReviewResponseModel></returns>
        public async Task<ReviewResponseModel> updateOneReview(ReviewRequestModel model)
        {
            //���N�G�X�g��������쐬            
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

            //���N�G�X�g�𑗐M���ă��X�|���X�擾
            var response = await _commonService.HttpClientFactory.PostAsync(ConstClass.GAS_API_URL, request);
            //���X�|���X���擾�ł��Ȃ����null��Ԃ�
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            //���X�|���X���I�u�W�F�N�g�ɉ��H
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ReviewResponseModel>(responseString);

            return responseObject;
        }
    }
}
