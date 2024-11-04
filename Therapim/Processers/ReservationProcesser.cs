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
    /// �\��p�v���Z�b�T�[�N���X
    /// </summary>
    public class ReservationProcesser
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly ICommonService _commonService;

        //DI�R���e�i���擾�����@�\�̃C���X�^���X�����������Ă���
        public ReservationProcesser(
            ILogger<ReviewController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        /// <summary>
        /// Gas API���������ė\���o�^����
        /// </summary>
        /// <returns>�\��o�^���� <ReviewResponseModel></returns>
        public async Task<ReservationResponseModel> createOneReservation(ReservationRequestModel model)
        {
            //�Z�b�V����ID�͂�����API�ւ̃��N�G�X�g�p�ɒǉ�����
            model.CreatedSessionId = _commonService.GetSessionSessionId();

            string jsonContentString = JsonConvert.SerializeObject(model, Formatting.Indented);
            var requestJson = @"{""targetFunc"":""reservation"", ""crudType"": ""create"", ""targetRange"": ""one"", ""sqlCond"": """",""input"": "
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
            var responseObject = JsonConvert.DeserializeObject<ReservationResponseModel>(responseString);

            return responseObject;
        }


    }
}
