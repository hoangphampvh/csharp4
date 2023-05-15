using assiment_csad4.Configruration;
using assiment_csad4.IService;
using assiment_csad4.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
/*
AddSingleton : Tạo ra 1 đối tượng service tồn tại cho đến khi vòng đời của ứng dụng kết thức: Service này sẽ được dùng chung cho các request 
loại đăng ký này cũng phù hợp với các service mang tính toàn cục và không thay đổi

AddScoped : Mỗi request cụ thể sẽ tạo ra 1 đối tượng service, đối tượng này đc giữ nguyên trong quá trình xử lý request. 
Phù hợp cho các services mà phục vụ cho 1 loại request
 */
builder.Services.AddTransient<IServiceCartDetail,CartDetailService>();
builder.Services.AddTransient<IServiceBill, BillService>();
builder.Services.AddTransient<IBillDetailService, BillDetailService>();
builder.Services.AddTransient<IServiceProduct, ProductService>();
builder.Services.AddTransient<IServiceUser, UserService>();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();            // Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)
//builder.Services.AddSession(cfg => {                    // Đăng ký dịch vụ Session
//    cfg.Cookie.Name = "Session_Key1";            // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
//    cfg.IdleTimeout = new TimeSpan(0, 30, 0); // Thời gian tồn tại của Session
//});
//builder.Services.AddSession(cfg => {                    // Đăng ký dịch vụ Session
//    cfg.Cookie.Name = "Session_Key2";            // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
//    cfg.IdleTimeout = new TimeSpan(0, 1, 0); // Thời gian tồn tại của Session
//});

var sessionTimes = new Dictionary<string, TimeSpan>()
{
    {"Session_Key1", new TimeSpan(0, 30, 0)}, // 30p
    {"Session_Key2", new TimeSpan(0, 30, 0)}, // 3s 
    {"Session_Key", new TimeSpan(0, 30, 0)}, // 3s 
};


// Đăng ký session vào dịch vụ
builder.Services.AddSession(options =>
{
    foreach (var key in sessionTimes.Keys)
    {
        TimeSpan duration;
        if (sessionTimes.TryGetValue(key, out duration))
        {
            options.IdleTimeout = duration;
        }
    }
});

builder.Services.AddTransient<CartDetailService>();
builder.Services.AddTransient<BillService>();
builder.Services.AddTransient<BillDetailService>();
builder.Services.AddTransient<ProductService>();
builder.Services.AddTransient<UserService>();


builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(x =>
                {
                    x.LoginPath = "/Account/Login";
                    x.ExpireTimeSpan = new TimeSpan(0, 30, 0);

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
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseAuthorization();
app.MapGet("/abc", async httpcontext =>
{
    await httpcontext.Response.WriteAsync("abc");
});
app.MapControllerRoute(
    name: "Index",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
