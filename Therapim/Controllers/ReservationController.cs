using Microsoft.AspNetCore.Mvc;
using Therapim.Models;
using Therapim.Processers;
using Therapim.Services;

namespace Therapim.Controllers
{
    /// <summary>
    /// �\��p�R���g���[���[�N���X
    /// </summary>
    public class ReservationController : Controller
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly ICommonService _commonService;

        //DI�R���e�i���擾�����@�\�̃C���X�^���X�����������Ă���
        public ReservationController(
            ILogger<ReviewController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }


        /// <summary>
        /// �\��o�^���
        /// </summary>
        /// <returns>�\��o�^���</returns>        
        [HttpGet]
        public async Task<IActionResult> Create(string? MenuId)
        {
            try
            {
                // �V�K�쐬�̏ꍇ�͋�̃��f����Ԃ��̂ō쐬
                var reservation = new ReservationRequestModel();

                // ���O�C�����̏ꍇ�͌l�����i�[���Ă���
                reservation.FullName = _commonService.GetCookieFullName();
                reservation.Birthday = _commonService.GetCookieBirthday();
                reservation.PhoneNumber = _commonService.GetCookiePhoneNumber();
                reservation.MailAddress = _commonService.GetCookieMailAddress();

                // ���j���[���擾����
                var CourseProcesser = new CourseProccesser(_logger, _commonService);
                var CourseList = await CourseProcesser.getCourseList();

                if (CourseList == null)
                {
                    // TempData �ɃG���[���b�Z�[�W��ݒ�
                    TempData["ErrorMessage"] = "���j���[���擾�̒ʐM�Ɏ��s���܂����B�ēx���������������Ă��������Ȃ��ꍇ�͓X�܂܂ł��₢���킹���������B";
                    // �G���[��ʂɃ��_�C���N�g
                    return RedirectToAction("Error", "Home");
                }

                ViewData["CourseList"] = CourseList;

                // ���j���[��ʂ��痈���ꍇ�̓p�����[�^�̃��j���[ID�擾���ēn��
                ViewData["MenuId"] = MenuId?.ToString() ?? string.Empty;

                return View("Create", reservation);
            }
            catch (Exception ex)
            {
                // TempData �ɃG���[���b�Z�[�W��ݒ�
                TempData["ErrorMessage"] = "�\���ʂ̎擾�Ɏ��s���܂����B�ēx���������������Ă��������Ȃ��ꍇ�͓X�܂܂ł��₢���킹���������B";
                // �G���[��ʂɃ��_�C���N�g
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        /// �\��o�^���(POSTBACK��)
        /// </summary>
        /// <returns>�\��o�^���(POSTBACK��)</returns>        
        [HttpPost]
        public async Task<IActionResult> Create(ReservationRequestModel model)
        {
            try
            {
                // ���j���[���擾����
                var CourseProcesser = new CourseProccesser(_logger, _commonService);
                var CourseList = await CourseProcesser.getCourseList();

                if (CourseList == null)
                {
                    // TempData �ɃG���[���b�Z�[�W��ݒ�
                    TempData["ErrorMessage"] = "�߂�ۂɃ��j���[���擾�̒ʐM�Ɏ��s���܂����B�ēx���������������Ă��������Ȃ��ꍇ�͓X�܂܂ł��₢���킹���������B";
                    // �G���[��ʂɃ��_�C���N�g
                    return RedirectToAction("Error", "Home");
                }

                ViewData["CourseList"] = CourseList;

                return View("Create", model);
            }
            catch (Exception ex)
            {
                // TempData �ɃG���[���b�Z�[�W��ݒ�
                TempData["ErrorMessage"] = "�\���ʂ̍Ď擾�Ɏ��s���܂����B�ēx���������������Ă��������Ȃ��ꍇ�͓X�܂܂ł��₢���킹���������B";
                // �G���[��ʂɃ��_�C���N�g
                return RedirectToAction("Error", "Home");
            }        
        }


        /// <summary>
        /// �\��o�^�m�F���
        /// </summary>
        /// <returns>�\��o�^�m�F���</returns>        
        [HttpPost]
        public async Task<IActionResult> CreateConfirm(ReservationRequestModel model)
        {
            try
            {
                //userId�͉�ʂɂȂ��Anull�ɂȂ��Ă��邽�߃Z�b�V������userId�ŏ㏑��(�K�{�ł͂Ȃ��̂�null�ł��悢)
                model.UserId = _commonService.GetSessionUserId();

                // �ēx�o���f�[�V���������s����ModelState���X�V����
                TryValidateModel(model);
                // �o���f�[�V�����G���[������ꍇ�͓��͉�ʂ��ĕ\��
                if (!ModelState.IsValid)
                {
                    // ���j���[���擾����
                    var CourseProcesser = new CourseProccesser(_logger, _commonService);
                    var CourseList = await CourseProcesser.getCourseList();

                    if (CourseList == null)
                    {
                        // TempData �ɃG���[���b�Z�[�W��ݒ�
                        TempData["ErrorMessage"] = "���j���[���擾�̒ʐM�Ɏ��s���܂����B�ēx���������������Ă��������Ȃ��ꍇ�͓X�܂܂ł��₢���킹���������B";
                        // �G���[��ʂɃ��_�C���N�g
                        return RedirectToAction("Error", "Home");
                    }

                    ViewData["CourseList"] = CourseList;
                    return View("Create", model);
                }

                // ���Ȃ���Ίm�F��ʂ֑J��
                return View("CreateConfirm", model);
            }
            catch (Exception ex)
            {
                // TempData �ɃG���[���b�Z�[�W��ݒ�
                TempData["ErrorMessage"] = "�\��m�F��ʂ̎擾�Ɏ��s���܂����B�ēx���������������Ă��������Ȃ��ꍇ�͓X�܂܂ł��₢���킹���������B";
                // �G���[��ʂɃ��_�C���N�g
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// �\��o�^�m�F���
        /// </summary>
        /// <returns>�\��o�^�m�F���</returns>        
        [HttpGet]
        public async Task<IActionResult> CreateConfirm()
        {
            // TempData �ɃG���[���b�Z�[�W��ݒ�
            TempData["ErrorMessage"] = "���O�C����ɓ��͒��̉�ʂɖ߂邱�Ƃ͂ł��܂���B���萔�ł������͂��ēx���肢�������܂��B";
            // �G���[��ʂɃ��_�C���N�g
            return RedirectToAction("Error", "Home");
        }
            /// <summary>
            /// �\��o�^����
            /// </summary>
            /// <returns>�\��o�^������̑J�ډ��</returns>
            [HttpPost]
        public async Task<IActionResult> XtCreate(ReservationRequestModel model)
        {
            try
            {
                //userId�͉�ʂɂȂ��Anull�ɂȂ��Ă��邽�߃Z�b�V������userId�ŏ㏑��(�K�{�ł͂Ȃ��̂�null�ł��悢)
                model.UserId = _commonService.GetSessionUserId();

                // �Z�b�V�����̃��[�UID���Ȃ��ꍇ�́A�N�b�L�[�̃��[�UID���i�[���Ă���
                if(String.IsNullOrEmpty(model.UserId))
                {
                    model.UserId = _commonService.GetCookieUserId();
                }

                // �ēx�o���f�[�V���������s����ModelState���X�V����
                TryValidateModel(model);
                // �o���f�[�V�����G���[������ꍇ�͊m�F��ʂ��ĕ\��
                if (!ModelState.IsValid)
                {
                    return View("CreateConfirm", model);
                }

                // �o�^����
                var reservationProcesser = new ReservationProcesser(_logger, _commonService);
                var createReservationResult = await reservationProcesser.createOneReservation(model);

                if (createReservationResult == null)
                {
                    // TempData �ɃG���[���b�Z�[�W��ݒ�
                    TempData["ErrorMessage"] = "�\��\���̒ʐM�Ɏ��s���܂����B�ēx���������������Ă��������Ȃ��ꍇ�͓X�܂܂ł��₢���킹���������B";
                    // �G���[��ʂɃ��_�C���N�g
                    return RedirectToAction("Error", "Home");
                }

                // �������͊�����ʂ�\��
                return View("CreateComplete", model);
            }
            catch (Exception ex)
            {
                // TempData �ɃG���[���b�Z�[�W��ݒ�
                TempData["ErrorMessage"] = "���\��o�^�̏����Ɏ��s���܂����B�ēx���������������Ă��������Ȃ��ꍇ�͓X�܂܂ł��₢���킹���������B";
                // �G���[��ʂɃ��_�C���N�g
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
