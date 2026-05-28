using AzureWebApi.DTOs;
using AzureWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IConfiguration _configuration;

        public EmployeeController(IEmployeeService employeeService, IConfiguration configuration, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllEmployees()
        {
            _logger.LogInformation("Fetching all employees");

            var employees = await _employeeService.GetAllEmployeesAsync();

            var greeting = _configuration["Greeting"];

            return Ok(new
            {
                Message = greeting,
                Data = employees
            });
        }

        /// <summary>
        /// Retrieves a single employee by ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching employee with ID: {Id}", id);
                var employee = await _employeeService.GetEmployeeByIdAsync(id);

                if (employee is null)
                {
                    _logger.LogWarning("Employee with ID: {Id} not found", id);
                    return NotFound(new { Message = $"Employee with ID {id} was not found." });
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching employee with ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }


        }
    }
}
