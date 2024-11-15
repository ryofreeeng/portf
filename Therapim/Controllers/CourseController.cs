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
    /// ���j���[�p�R���g���[���[�N���X
    /// </summary>
    public class CourseController : Controller
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly ICommonService _commonService;

        //DI�R���e�i���擾�����@�\�̃C���X�^���X�����������Ă���
        public CourseController(
            ILogger<ReviewController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        /// <summary>
        /// ���j���[�ꗗ��ʕ\��
        /// </summary>
        /// <returns>���j���[�ꗗ��� with <CourseResponseModel></returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            //�擾����
            var CourseProcesser = new CourseProccesser(_logger, _commonService);
            var CourseList = await CourseProcesser.getCourseList();

            if(CourseList == null)
            {
                // TempData �ɃG���[���b�Z�[�W��ݒ�
                TempData["ErrorMessage"] = "���j���[���擾�̒ʐM�Ɏ��s���܂����B�ēx���������������Ă��������Ȃ��ꍇ�͓X�܂܂ł��₢���킹���������B";
                // �G���[��ʂɃ��_�C���N�g
                return RedirectToAction("Error", "Home");
            }
            //�ꗗ��ʂŁA���X�񐔂Ɩ��O��\��
            ViewData["VisitedTimes"] = _commonService.GetSessionVisitedTimes();
            ViewData["FullName"] = _commonService.GetCookieFullName();
            ViewData["WelcomeMessage"] = "����1�񂲗��X���������ƃ|�C���g�����܂�܂��B";

            return View(CourseList);
            //return View();
        }
    }
}
