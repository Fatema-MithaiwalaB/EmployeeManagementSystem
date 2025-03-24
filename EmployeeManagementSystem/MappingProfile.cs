using AutoMapper;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EmployeeCreateOrUpdateDto, Employee>();
        CreateMap<Employee, EmployeeResponseDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName));
    }
}
