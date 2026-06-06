using AzureWebApi.Core.Entities;
using AzureWebApi.Core.Interfaces;
using AzureWebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AzureWebApi.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
            => await _context.Employees
                             .Where(e => e.IsActive)
                             .AsNoTracking()
                             .ToListAsync();

        public async Task<Employee?> GetByIdAsync(int id)
            => await _context.Employees
                             .AsNoTracking()
                             .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

        public async Task<Employee> CreateAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> UpdateAsync(int id, Employee updated)
        {
            var existing = await _context.Employees.FindAsync(id);
            if (existing == null || !existing.IsActive) return null;

            existing.FirstName = updated.FirstName;
            existing.LastName = updated.LastName;
            existing.Email = updated.Email;
            existing.Department = updated.Department;
            existing.Designation = updated.Designation;
            existing.Salary = updated.Salary;
            existing.DateOfJoining = updated.DateOfJoining;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            // Soft delete — enterprise best practice
            employee.IsActive = false;
            employee.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
            => await _context.Employees
                             .AnyAsync(e => e.Email == email
                                         && e.IsActive
                                         && (!excludeId.HasValue || e.Id != excludeId.Value));
    }
}
