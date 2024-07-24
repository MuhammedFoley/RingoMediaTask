using System.ComponentModel.DataAnnotations;

namespace RingoMedia.PL.ViewModels
{
    public class DepartmentCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public IFormFile Logo { get; set; }

        public int? ParentDepartmentId { get; set; }
    }

}

