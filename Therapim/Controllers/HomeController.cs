using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Therapim.Filters;
using Therapim.Models;

namespace Therapim.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
//            HttpContext.Session.Remove("SessionId");
  //          HttpContext.Session.Remove("UserId");


            var testUserId = HttpContext.Session.GetString("UserId");
            _logger.LogInformation("������Index : �Z�b�V�����̃��[�UID�@" + testUserId);

            var testSessionId = HttpContext.Session.GetString("SessionId");
            _logger.LogInformation("������Index : �Z�b�V�����̃Z�b�V����ID�@" + testSessionId);

            _logger.LogWarning("������Index");
            return View();
        }

        //[PermissionAuthorize(false, "adult", "silver", "age30s")] // �����ꂩ�̌����^�C�v���K�v
        //[PermissionAuthorize(true, "adult", "silver", "age30s")] // �S�Ă̌����^�C�v���K�v
        //[RankAuthorize(3)] // ���������N3�ȏオ�K�v
        public IActionResult Privacy()
        {

            var testUserId = HttpContext.Session.GetString("UserId");
            _logger.LogInformation("������XtLogin : �i�[���Ď擾�������[�UID�@" + testUserId);

            var testSessionId = HttpContext.Session.GetString("SessionId");
            _logger.LogInformation("������XtLogin : �i�[���Ď擾�����Z�b�V����ID�@" + testSessionId);

            _logger.LogWarning("������Privacy");
            return View();
        }


        public IActionResult Error()
        {
            // TempData ����G���[���b�Z�[�W���擾
            var errorMessage = TempData["ErrorMessage"] as string;

            // �G���[���b�Z�[�W�����݂���ꍇ�A�r���[�ɓn��
            return View(new ErrorViewModel { Message = errorMessage });
        }

        /*
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        */
    }
}
