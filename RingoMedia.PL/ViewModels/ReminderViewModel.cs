using System.ComponentModel.DataAnnotations;

namespace RingoMedia.PL.ViewModels
{
    public class ReminderViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime ReminderDateTime { get; set; }
    }
}
