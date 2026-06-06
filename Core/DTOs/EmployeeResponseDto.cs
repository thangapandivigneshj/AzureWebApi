namespace AzureWebApi.Core.DTOs
{
    public class EmployeeResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime DateOfJoining { get; set; }
        public bool IsActive { get; set; }
    }
}
