using AzureWebApi.DTOs;

namespace AzureWebApi.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IReadOnlyList<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
    }
}
