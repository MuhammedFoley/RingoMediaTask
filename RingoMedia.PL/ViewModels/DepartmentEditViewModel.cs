using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RingoMedia.PL.ViewModels
{
    public class DepartmentEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile? Logo { get; set; } // Nullable if no new file is being uploaded
        public int? ParentDepartmentId { get; set; }
        public IEnumerable<SelectListItem>? Departments { get; set; } // For dropdown list
    }



}
