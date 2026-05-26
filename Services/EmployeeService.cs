using AzureWebApi.DTOs;
using AzureWebApi.Services.Interfaces;

namespace AzureWebApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private static readonly IReadOnlyList<EmployeeDto> _employees = new List<EmployeeDto>
        {
            new() { Id = 1, FullName = "Vignesh Kumar",   Email = "vignesh@company.com",  Department = "Engineering",  Designation = "Senior .NET Developer", Salary = 85000, IsActive = true  },
            new() { Id = 2, FullName = "Anitha Rajan",    Email = "anitha@company.com",   Department = "HR",           Designation = "HR Manager",            Salary = 70000, IsActive = true  },
            new() { Id = 3, FullName = "Karthik Selvam",  Email = "karthik@company.com",  Department = "Engineering",  Designation = "QA Engineer",           Salary = 60000, IsActive = true  },
            new() { Id = 4, FullName = "Priya Mohan",     Email = "priya@company.com",    Department = "Finance",      Designation = "Finance Analyst",        Salary = 65000, IsActive = false },
            new() { Id = 5, FullName = "Suresh Babu",     Email = "suresh@company.com",   Department = "Engineering",  Designation = "DevOps Engineer",        Salary = 78000, IsActive = true  },
        };

        public Task<IReadOnlyList<EmployeeDto>> GetAllEmployeesAsync()
        {
            return Task.FromResult(_employees);
        }

        public Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(employee);
        }
    }
}
