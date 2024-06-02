using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SignalRChat.Hubs;
using SignalRChat.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(); // 添加SignalR服务

//builder.Services.AddSignalR().AddStackExchangeRedis(options =>
//{
//    options.Configuration.ClientName = "SignalR";
//});

//builder.Services.AddSignalR().AddAzureSignalR();


var Configuration = builder.Configuration;

// 注册数据库上下文服务
builder.Services.AddDbContext<MnDbContext>(options =>
 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

// 配置JWT身份验证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "croptoking", // 替换为您的颁发者
            ValidAudience = "autologoutapp", // 替换为您的受众
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("wu12345007008")) // 替换为您的密钥
        };
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chatHub"); // 配置SignalR Hub的端点
    endpoints.MapHub<NotificationHub>("/notificationHub");
    endpoints.MapHub<MessageHub>("/messages");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});



app.Run();
