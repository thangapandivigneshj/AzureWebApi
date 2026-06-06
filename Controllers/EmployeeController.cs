using AutoMapper;
using AzureWebApi.Core.DTOs;
using AzureWebApi.Core.Entities;
using AzureWebApi.Core.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AzureWebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<EmployeeRequestDto> _validator;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(
            IEmployeeRepository repository,
            IMapper mapper,
            IValidator<EmployeeRequestDto> validator,
            ILogger<EmployeesController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        // GET api/v1/employees
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmployeeResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _repository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
            return Ok(result);
        }

        // GET api/v1/employees/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee is null)
                return NotFound(new { Message = $"Employee with ID {id} not found." });

            return Ok(_mapper.Map<EmployeeResponseDto>(employee));
        }

        // POST api/v1/employees
        [HttpPost]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] EmployeeRequestDto request)
        {
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            if (await _repository.ExistsByEmailAsync(request.Email))
                return Conflict(new { Message = "Email already registered." });

            var entity = _mapper.Map<Employee>(request);
            var created = await _repository.CreateAsync(entity);
            var response = _mapper.Map<EmployeeResponseDto>(created);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        // PUT api/v1/employees/{id}
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeRequestDto request)
        {
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            if (await _repository.ExistsByEmailAsync(request.Email, excludeId: id))
                return Conflict(new { Message = "Email already used by another employee." });

            var updated = await _repository.UpdateAsync(id, _mapper.Map<Employee>(request));
            if (updated is null)
                return NotFound(new { Message = $"Employee with ID {id} not found." });

            return Ok(_mapper.Map<EmployeeResponseDto>(updated));
        }

        // DELETE api/v1/employees/{id}
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"Employee with ID {id} not found." });

            return NoContent();
        }
    }
}
