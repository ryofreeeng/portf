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
    /// ���j���[�p�v���Z�b�T�[�N���X
    /// </summary>
    public class CourseProccesser
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly ICommonService _commonService;

        //DI�R���e�i���擾�����@�\�̃C���X�^���X�����������Ă���
        public CourseProccesser(
            ILogger<ReviewController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        /// <summary>
        /// Gas API��胁�j���[�ꗗ���擾����
        /// </summary>
        /// <returns>���j���[�ꗗ List<CourseResponseModel></returns>
        public async Task<CourseResponseModel> getCourseList()
        {
            //���N�G�X�g��������쐬
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

            //���N�G�X�g�𑗐M���ă��X�|���X�擾
            var response = await _commonService.HttpClientFactory.PostAsync(ConstClass.GAS_API_URL, request);
            //���X�|���X���擾�ł��Ȃ����null��Ԃ�
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            //���X�|���X���I�u�W�F�N�g�ɉ��H
            var responseString = await response.Content.ReadAsStringAsync();            
            var responseObject = JsonConvert.DeserializeObject<CourseResponseModel>(responseString);

            // ���X�|���X�f�[�^�����H����@���H���@����̂��ߕۗ�
            //var responseObjectVH = ProcessResponseData(responseObject);
            var responseObjectVH = responseObject;

            return responseObjectVH;
        }


    }
}
