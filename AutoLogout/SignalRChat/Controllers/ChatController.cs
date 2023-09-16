using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;
using SignalRChat.Models;
using SignalRChat.ViewModels;
using System.Threading.Tasks;

namespace SignalRChat.Controllers
{
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageViewModel message)
        {
            if (ModelState.IsValid)
            {
                // 处理消息发送逻辑
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.User, message.Text);
                return RedirectToAction("Index");
            }
            return View("Index", message);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(MessageViewModel message)
        {
            // 处理用户退出登录逻辑
            // 可以在这里通知WPF客户端退出登录
            // 例如，发送一个退出命令给WPF客户端
            await _hubContext.Clients.All.SendAsync("Logout", message.User, message.Text);
            return RedirectToAction("Index");
        }
    }
}
