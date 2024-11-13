using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Therapim.Controllers;


///このクラスは使わないことにした
///テスト観点から、コントローラクラスの継承ではなく、サービスクラスを作成して依存性注入して使用する



/// <summary>
/// すべてのコントローラで共通する処理
/// </summary>
public abstract class BaseController : Controller
{
    //ログ機能のインスタンス宣言
    private readonly ILogger<BaseController> _logger;

    //DIコンテナより取得したログ機能を用いてインスタンスを初期化しておく
    public BaseController(ILogger<BaseController> logger)
    {
        _logger = logger;        
    }

    /// <summary>
    /// フィールド定義。アンダースコアをつけたprivateなフィールドを定義して、取得や変更はプロパティで行う
    /// 変更することはまだ想定していないが、一応変更可能にしている
    /// </summary>
    private string _controllerName;
    private string _actionName;
    private string _userIdInSession;
    private string _sessionIdInSession;
    private string _userIdInCookie;
    private string _sessionIdInCookie;


    /// <summary>
    /// プロパティ定義。各フィールドへのアクセスやセットはこちらで行う
    /// </summary>
    protected ILogger<BaseController> Logger
    {
        get { return _logger; }
    }
    
    protected string ControllerName
    {
        get { return _controllerName; }
        set { _controllerName = value; }
    }

    protected string ActionName
    {
        get { return _actionName; }
        set { _actionName = value; }
    }

    protected string UserIdInSession
    {
        get { return _userIdInSession; }
        set { _userIdInSession = value; }
    }

    protected string SessionIdInSession
    {
        get { return _sessionIdInSession; }
        set { _sessionIdInSession = value; }
    }

    protected string UserIdInCookie
    {
        get { return _userIdInCookie; }
        set { _userIdInCookie = value; }
    }

    protected string SessionIdInCookie
    {
        get { return _sessionIdInCookie; }
        set { _sessionIdInCookie = value; }
    }




    //var birthday = User.FindFirst("Birthday")?.Value;
    //var telNo = User.FindFirst("TelNo")?.Value;



    //アクションメソッド実行前の処理
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        //コントローラ名とアクション名を取得しておく
        ControllerName = ControllerContext.ActionDescriptor.ControllerName;
        ActionName = ControllerContext.ActionDescriptor.ActionName;

        //セッション情報とクッキー情報を設定取得しておく
        UserIdInSession = HttpContext.Session.GetString("UserId");
        SessionIdInSession = HttpContext.Session.GetString("SessionId");
        UserIdInCookie = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        SessionIdInCookie = User.FindFirst(ClaimTypes.Name)?.Value;

        base.OnActionExecuting(context);
    }
}
