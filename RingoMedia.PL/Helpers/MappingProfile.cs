using AutoMapper;
using RingoMedia.DAL.Models;
using RingoMedia.PL.ViewModels;

namespace RingoMedia.PL.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Department, DepartmentCreateViewModel>().ReverseMap();
            CreateMap<Department, DepartmentEditViewModel>().ReverseMap();
            CreateMap<Department, DepartmentDetailsViewModel>()
                .ForMember(dest => dest.ParentDepartmentName, opt => opt.MapFrom(src => src.ParentDepartment.Name))
                .ForMember(dest => dest.SubDepartments, opt => opt.MapFrom(src => src.SubDepartments));
            CreateMap<Department, SubDepartmentViewModel>();
            CreateMap<Reminder, ReminderViewModel>().ReverseMap();
        }
    }

}
