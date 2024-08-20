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
    /// ���r���[�p�v���Z�b�T�[�N���X
    /// </summary>
    public class LoginProccesser
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ICommonService _commonService;

        //DI�R���e�i���擾�����@�\�̃C���X�^���X�����������Ă���
        public LoginProccesser(
            ILogger<LoginController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        /// <summary>
        /// Gas API���ڋq�����擾����
        /// </summary>
        /// <returns>�ڋq���@<LoginResponseModel> </returns>
        public async Task<LoginResponseModel> getOneCustomer(LoginRequestModel model)
        {
            //SQL�p�̕�������쐬
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

            //���N�G�X�g�𑗐M���ă��X�|���X�擾
            var response = await _commonService.HttpClientFactory.PostAsync(ConstClass.GAS_API_URL, request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            //���Ԃ񂢂�Ȃ�����
            //var responseString = await response.Content.ReadAsStringAsync();
            //var responseObject = JsonConvert.DeserializeObject<LoginResponseModel>(responseString);

            var users = await response.Content.ReadFromJsonAsync<LoginResponseModel>();

            return users;
        }


        /// <summary>
        /// ���O�C���������s��
        /// </summary>
        /// <returns>�Ȃ�</returns>
        public async Task<bool> executeLogin(LoginResponseModel users)
        {
            //���ʕԋp�p�ϐ�
            bool loginResult = false;

            //�ڋq��񂪎擾�ł����ꍇ�̂݃��O�C������
            if (users.SqlContent.Count != 0)
            {
                var user = users.SqlContent[0];
                _logger.LogInformation($"������API����擾�������[�UID�F{user.UserId}");

                //�Z�b�V����ID����
                var sessionId = "session_" + user.UserId + "_" + DateTime.Now;

                // �N�b�L�[�Ɋi�[����N���[����񃊃X�g���쐬�BPermission�͌ォ��ǉ����������邽�߁A�z��ł͂Ȃ��T�C�Y�ύX�\��List�ō쐬����
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId),
                    new Claim("SessionId",sessionId),
                    new Claim(ClaimTypes.Name,user.FullName),
                    new Claim("Birthday",user.Birthday.ToString()),
                    new Claim("PhoneNumber",user.PhoneNumber),
                };
                //Permission��������o���ăN���[����񃊃X�g�ɒǉ�
                if (!string.IsNullOrEmpty(user.Permission))
                {
                    claims.AddRange(
                        user.Permission.Split(',')
                                        .Select(permission => new Claim("Permission", permission))
                    );
                }

                //�N�b�L�[�F�؂��w�肵�ď���ݒ�
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // �i���I�ȃN�b�L�[�ɂ��邩�ǂ����i�u���E�U����Ă��c�邩�ǂ����j
                    ExpiresUtc = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo")).AddHours(24) // �N�b�L�[�̗L��������1���ɐݒ�B�匳�̐ݒ�����邪������ŏ㏑�����Ă�����
                };

                // ���[�U�[���T�C���C��
                await _commonService.HttpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                // �Z�b�V�����Ƀ��[�U�[����ۑ�
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
