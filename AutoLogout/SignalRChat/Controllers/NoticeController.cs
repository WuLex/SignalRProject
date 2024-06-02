using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;
using SignalRChat.Models;

namespace SignalRChat.Controllers
{
    public class NoticeController : Controller
    {
        private readonly List<Notice> _notices = new List<Notice>();
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly MnDbContext _mnDbContext;
        public NoticeController(IHubContext<NotificationHub> hubContext, MnDbContext mnDbContext)
        {
            _hubContext = hubContext;
            _mnDbContext = mnDbContext; 
        }

        public IActionResult Index()
        {
            _notices.AddRange(_mnDbContext.Notices.ToList());
            return View(_notices);
        }

        [HttpPost]
        public async Task<IActionResult> PostNotice(Notice notice)
        {
            if (ModelState.IsValid)
            {
                notice.CreatedAt = DateTime.Now;
                _mnDbContext.Notices.Add(notice); 
                await _mnDbContext.SaveChangesAsync();
                // 实时推送到客户端
                await _hubContext.Clients.AllExcept("admin").SendAsync("ReceiveNotice", notice);

                return Ok(new { success = true });
            }

            return BadRequest(new { error = "无效的提交数据" });
           
        }
    }
}
