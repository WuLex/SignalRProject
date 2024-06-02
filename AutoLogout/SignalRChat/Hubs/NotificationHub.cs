using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class NotificationHub : Hub
    { // 客户端连接时执行
        public override async Task OnConnectedAsync()
        {
            // 可以在这里添加一些逻辑，比如向客户端发送欢迎消息等
            await Clients.Client(Context.ConnectionId).SendAsync("WelcomeMessage", "Welcome to the notification hub!");
            await base.OnConnectedAsync();
        }

        // 客户端断开连接时执行
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // 可以在这里添加一些逻辑，比如记录日志等
            await base.OnDisconnectedAsync(exception);
        }

        // 客户端发送消息时执行
        public async Task SendMessage(string user, string message)
        {
            // 将消息发送给所有连接的客户端
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveMessage", user, message);
        }
    }
}
