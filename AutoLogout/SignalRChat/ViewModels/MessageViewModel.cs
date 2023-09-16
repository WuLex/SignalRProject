using System.ComponentModel.DataAnnotations;

namespace SignalRChat.ViewModels
{
    public class MessageViewModel
    {
        [Required]
        public string User { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
