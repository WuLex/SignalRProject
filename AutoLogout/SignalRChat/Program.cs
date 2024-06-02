using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SignalRChat.Hubs;
using SignalRChat.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(); // ���SignalR����

//builder.Services.AddSignalR().AddStackExchangeRedis(options =>
//{
//    options.Configuration.ClientName = "SignalR";
//});

//builder.Services.AddSignalR().AddAzureSignalR();


var Configuration = builder.Configuration;

// ע�����ݿ������ķ���
builder.Services.AddDbContext<MnDbContext>(options =>
 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

// ����JWT�����֤
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "croptoking", // �滻Ϊ���İ䷢��
            ValidAudience = "autologoutapp", // �滻Ϊ��������
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("wu12345007008")) // �滻Ϊ������Կ
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
    endpoints.MapHub<ChatHub>("/chatHub"); // ����SignalR Hub�Ķ˵�
    endpoints.MapHub<NotificationHub>("/notificationHub");
    endpoints.MapHub<MessageHub>("/messages");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});



app.Run();
