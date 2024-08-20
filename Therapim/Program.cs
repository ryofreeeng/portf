using Microsoft.AspNetCore.Authentication.Cookies;
using Therapim.Filters;
using Therapim.Services;

var builder = WebApplication.CreateBuilder(args);

// Kestrel �T�[�o�[�̐ݒ�
/*
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5172); // �|�[�g�ԍ����w��
    // options.Listen(IPAddress.Parse("0.0.0.0"), 5000); // �����IP�Ƀo�C���h����ꍇ
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

//httpClient��DI�R���e�i�ɒǉ�
builder.Services.AddHttpClient();
//HttpContext�ɃA�N�Z�X���邽�߂�HttpContextAccessor��DI�R���e�i�ɒǉ�
builder.Services.AddHttpContextAccessor();

//���ʃT�[�r�X��ǉ�
builder.Services.AddScoped<ICommonService, CommonService>();
//builder.Services.AddScoped<LoginAuthorizeAttribute>(); //ServiceFilter�Ƃ��Ďg�p����ꍇ�ɂ͕K�v�����ATypeFilter�ɂ����̂ŕs�v

//�Z�b�V�����Ǘ���ǉ�
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24); // �Z�b�V�����̃^�C���A�E�g��24���Ԃɐݒ�
    options.Cookie.HttpOnly = true; //javascript����N�b�L�[�ւ̃A�N�Z�X���֎~
    options.Cookie.IsEssential = true; //�N�b�L�[�ۑ��Ƀ��[�U�[�̋���K�v�Ƃ��Ȃ�
});

//�F�؃T�[�r�X�ǉ�
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index"; // ���O�C���y�[�W�̃p�X
        options.LogoutPath = "/Home/Index"; // ���O�A�E�g�y�[�W�̃p�X
        options.ExpireTimeSpan = TimeSpan.FromHours(24); // �N�b�L�[�̗L��������24���Ԃɐݒ�
        options.SlidingExpiration = true; // �X���C�f�B���O�L��������L���ɂ���
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

app.UseSession(); //�Z�b�V�����p�~�h���E�F�A��ǉ�

app.UseAuthentication(); // �F�؃~�h���E�F�A�̎g�p
app.UseAuthorization(); // �F�~�h���E�F�A�̎g�p

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
