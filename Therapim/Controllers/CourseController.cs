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
    /// ���r���[�p�R���g���[���[�N���X
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
        /// �R�[�X�ꗗ��ʕ\��
        /// </summary>
        /// <returns>�R�[�X�ꗗ��� with <CourseResponseModel></returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            //�擾����
            //var CourseProcesser = new CourseProccesser(_logger, _commonService);
            //var CourseList = await CourseProcesser.getCourseList();

            //if(CourseList == null)
            //{
                // TempData �ɃG���[���b�Z�[�W��ݒ�
                //TempData["ErrorMessage"] = "�R�[�X���擾�̒ʐM�Ɏ��s���܂����B�ēx���������������Ă��������Ȃ��ꍇ�͓X�܂܂ł��₢���킹���������B";
                // �G���[��ʂɃ��_�C���N�g
                //return RedirectToAction("Error", "Home");
            //}
            //�ꗗ��ʂŁA���[�UID����v���郌�r���[�ɂ͕ҏW�{�^����\�����邽�߂̃p�����[�^����������
            ViewData["VisitedTimes"] = 3;
            ViewData["WelcomeMessage"] = "����1�񂲗��X���������ƃ|�C���g�����܂�܂��B";

            //return View(CourseList);
            return View();
        }


        /// <summary>
        /// ���r���[�o�^���
        /// </summary>
        /// <returns>���r���[�o�^���</returns>
        [TypeFilter(typeof(LoginAuthorizeAttribute))]
        [HttpGet]
        public IActionResult Create()
        {
            // �V�K�쐬�̏ꍇ�͋�̃��f����Ԃ�
            var review = new ReviewRequestModel();
            return View("CreateOrEdit",review);
        }

        /// <summary>
        /// ���r���[�o�^����
        /// </summary>
        /// <returns>���r���[�o�^������̑J�ډ��</returns>
        [TypeFilter(typeof(LoginAuthorizeAttribute))]
        [HttpPost]
        public async Task<IActionResult> Create(ReviewRequestModel model)
        {
            //userId�͉�ʂɂȂ��Anull�ɂȂ��Ă��邽�߃Z�b�V������userId�ŏ㏑��
            var userId = _commonService.GetSessionUserId();
            if (string.IsNullOrEmpty(userId))
            {
                // �G���[�n���h�����O
                ModelState.AddModelError(string.Empty, "���O�C����񂪌Â��Ȃ��Ă��܂��B���O�C�����Ȃ����Ă�������");
                return View("CreateOrEdit", model);
            }
            model.UserId = userId;

            // �ēx�o���f�[�V���������s����ModelState���X�V����
            TryValidateModel(model);
            // �o���f�[�V�����G���[������ꍇ�͐V�K�o�^��ʂ��ĕ\��
            if (!ModelState.IsValid)
            {
                return View("CreateOrEdit", model);
            }

            // �o�^����
            var reviewProcesser = new ReviewProccesser(_logger, _commonService);
            var createReviewResult = await reviewProcesser.createOneReview(model);

            if (createReviewResult == null)
            {
                // TempData �ɃG���[���b�Z�[�W��ݒ�
                TempData["ErrorMessage"] = "���r���[�o�^�̒ʐM�Ɏ��s���܂����B�ēx���������������Ă��������Ȃ��ꍇ�͓X�܂܂ł��₢���킹���������B";
                // �G���[��ʂɃ��_�C���N�g
                return RedirectToAction("Error", "Home");
            }

            // �������͈ꗗ��ʂփ��_�C���N�g
            return RedirectToAction("List");
        }



        /// <summary>
        /// ���r���[�ҏW���
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns>���r���[�ҏW���</returns>
        [TypeFilter(typeof(LoginAuthorizeAttribute))]
        [HttpGet]
        public async Task<IActionResult> Edit(int reviewId)
        {
            // ���r���[ID�����݂��Ȃ��ꍇ�͐V�K�o�^�Ƀ��_�C���N�g
            if (reviewId <= 0)
            {                
                return RedirectToAction("Create");
            }

            // �ҏW�Ώۂ̃��r���[���擾����
            var reviewProcesser = new ReviewProccesser(_logger, _commonService);
            var review = await reviewProcesser.getOneReviewForEdit(reviewId);

            if(review == null)
            {
                // TempData �ɃG���[���b�Z�[�W��ݒ�
                TempData["ErrorMessage"] = "�ҏW���郌�r���[���擾�̒ʐM�Ɏ��s���܂����B�ēx���������������Ă��������Ȃ��ꍇ�͓X�܂܂ł��₢���킹���������B";
                // �G���[��ʂɃ��_�C���N�g
                return RedirectToAction("Error", "Home");
            }
            //�擾���ꂽ���r���[�̃��[�UID�����݂̃��O�C�����ƈقȂ��Ă�����G���[
            if (review.UserId != _commonService.GetSessionUserId()) {
                // TempData �ɃG���[���b�Z�[�W��ݒ�
                TempData["ErrorMessage"] = "���݂̃��O�C�����ō쐬���ꂽ���r���[�ł͂Ȃ����߁A���O�C�����Ȃ����Ă��������B�܂��͓X�܂܂ł��₢���킹�������B";
                // �G���[��ʂɃ��_�C���N�g
                return RedirectToAction("Error", "Home");
            }

            return View("CreateOrEdit",review);
        }


        /// <summary>
        /// ���r���[�X�V����
        /// </summary>
        /// <returns>���r���[�X�V������̑J�ډ��</returns>
        [TypeFilter(typeof(LoginAuthorizeAttribute))]
        [HttpPost]
        public async Task<IActionResult> Edit(ReviewRequestModel model)
        {
            //userId�͉�ʂɂȂ��Anull�ɂȂ��Ă��邽�߃Z�b�V������userId�ŏ㏑��
            var userId = _commonService.GetSessionUserId();
            if (string.IsNullOrEmpty(userId))
            {
                // �G���[�n���h�����O
                ModelState.AddModelError(string.Empty, "���O�C����񂪌Â��Ȃ��Ă��܂��B���O�C�����Ȃ����Ă�������");
                return View("CreateOrEdit", model);
            }
            model.UserId = userId;

            // �ēx�o���f�[�V���������s����ModelState���X�V����
            TryValidateModel(model);
            // �o���f�[�V�����G���[������ꍇ�͕ҏW��ʂ��ĕ\��
            if (!ModelState.IsValid)
            {
                return View("CreateOrEdit", model);
            }

            // �X�V����
            var reviewProcesser = new ReviewProccesser(_logger, _commonService);
            var updateReviewResult = await reviewProcesser.updateOneReview(model);

            if (updateReviewResult == null)
            {
                // TempData �ɃG���[���b�Z�[�W��ݒ�
                TempData["ErrorMessage"] = "���r���[���X�V�̒ʐM�Ɏ��s���܂����B�ēx���������������Ă��������Ȃ��ꍇ�͓X�܂܂ł��₢���킹���������B";
                // �G���[��ʂɃ��_�C���N�g
                return RedirectToAction("Error", "Home");
            }

            // �������͈ꗗ��ʂփ��_�C���N�g
            return RedirectToAction("List");
        }


}
}
