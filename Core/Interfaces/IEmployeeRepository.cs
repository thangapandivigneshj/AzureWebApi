using AzureWebApi.Core.Entities;

namespace AzureWebApi.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee?> UpdateAsync(int id, Employee employee);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByEmailAsync(string email, int? excludeId = null);
    }
}
