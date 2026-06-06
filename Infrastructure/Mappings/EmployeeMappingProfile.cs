using AutoMapper;
using AzureWebApi.Core.DTOs;
using AzureWebApi.Core.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AzureWebApi.Infrastructure.Mappings
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<EmployeeRequestDto, Employee>();

            CreateMap<Employee, EmployeeResponseDto>()
                .ForMember(dest => dest.FullName,
                           opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}
