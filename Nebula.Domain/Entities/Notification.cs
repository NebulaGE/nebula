using System; 

namespace Nebula.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsOpen { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime SendDate { get; set; }
        public NotificationType? Type { get; set; }
    }

    public enum NotificationType
    {
        Plain = 1,
        FromTeacher = 2
    }
}