using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingoMedia.DAL.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LogoPath { get; set; }
        public int? ParentDepartmentId { get; set; }
        public Department? ParentDepartment { get; set; }
        public ICollection<Department>? SubDepartments { get; set; } = new List<Department>();
    }

}
