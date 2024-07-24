using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingoMedia.DAL.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public string Title { get; set; }

        private DateTime _reminderDateTime;
        public DateTime ReminderDateTime
        {
            get => _reminderDateTime;
            set => _reminderDateTime = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
    }

}
