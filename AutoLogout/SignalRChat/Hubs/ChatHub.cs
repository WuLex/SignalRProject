using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // 处理从客户端接收到的消息
            // 并广播消息给所有连接的客户端
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task Logout(string user, string message)
        {
            await Clients.All.SendAsync("Logout", user, message);
        }
    }
}
