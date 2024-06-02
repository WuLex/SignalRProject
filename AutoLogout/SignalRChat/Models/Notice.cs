namespace SignalRChat.Models
{
    public class Notice
    {
        public int NoticeID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
