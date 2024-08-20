using Microsoft.AspNetCore.Authentication.Cookies;
using Therapim.Filters;
using Therapim.Services;

var builder = WebApplication.CreateBuilder(args);

// Kestrel サーバーの設定
/*
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5172); // ポート番号を指定
    // options.Listen(IPAddress.Parse("0.0.0.0"), 5000); // 特定のIPにバインドする場合
});

*/

// Add services to the container.
builder.Services.AddControllersWithViews();

/*
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});
*/

//httpClientをDIコンテナに追加
builder.Services.AddHttpClient();
//HttpContextにアクセスするためのHttpContextAccessorをDIコンテナに追加
builder.Services.AddHttpContextAccessor();

//共通サービスを追加
builder.Services.AddScoped<ICommonService, CommonService>();
//builder.Services.AddScoped<LoginAuthorizeAttribute>(); //ServiceFilterとして使用する場合には必要だが、TypeFilterにしたので不要

//セッション管理を追加
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24); // セッションのタイムアウトを24時間に設定
    options.Cookie.HttpOnly = true; //javascriptからクッキーへのアクセスを禁止
    options.Cookie.IsEssential = true; //クッキー保存にユーザーの許可を必要としない
});

//認証サービス追加
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index"; // ログインページのパス
        options.LogoutPath = "/Home/Index"; // ログアウトページのパス
        options.ExpireTimeSpan = TimeSpan.FromHours(24); // クッキーの有効期限を24時間に設定
        options.SlidingExpiration = true; // スライディング有効期限を有効にする
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); //セッション用ミドルウェアを追加

app.UseAuthentication(); // 認証ミドルウェアの使用
app.UseAuthorization(); // 認可ミドルウェアの使用

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
