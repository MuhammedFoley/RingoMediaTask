namespace RingoMedia.PL.ViewModels
{
    public class DepartmentDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LogoPath { get; set; }
        public string ParentDepartmentName { get; set; }
        public IEnumerable<SubDepartmentViewModel> SubDepartments { get; set; }
    }

    public class SubDepartmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
