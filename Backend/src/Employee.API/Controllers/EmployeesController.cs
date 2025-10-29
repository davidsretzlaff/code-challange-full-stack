using System.Threading;
using System.Threading.Tasks;
using EmployeeManagement.Application.Employees.UseCases.Create;
using EmployeeManagement.Application.Employees.UseCases.Delete;
using EmployeeManagement.Application.Employees.UseCases.GetById;
using EmployeeManagement.Application.Employees.UseCases.GetAll;
using EmployeeManagement.Application.Employees.UseCases.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public sealed class EmployeesController : ControllerBase
    {
        private readonly CreateEmployeeUseCase _create;
        private readonly GetEmployeeByIdUseCase _getById;
        private readonly GetAllEmployeesUseCase _getAll;
        private readonly UpdateEmployeeUseCase _update;
        private readonly DeleteEmployeeUseCase _delete;

        public EmployeesController(
            CreateEmployeeUseCase create,
            GetEmployeeByIdUseCase getById,
            GetAllEmployeesUseCase getAll,
            UpdateEmployeeUseCase update,
            DeleteEmployeeUseCase delete)
        {
            _create = create;
            _getById = getById;
            _getAll = getAll;
            _update = update;
            _delete = delete;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateEmployeeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request, CancellationToken ct)
        {
            var result = await _create.ExecuteAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Employee.Id }, result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(GetEmployeeByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken ct)
        {
            var result = await _getById.ExecuteAsync(id, ct);
            if (result.Employee is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllEmployeesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _getAll.ExecuteAsync(ct);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(UpdateEmployeeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateEmployeeRequest request, CancellationToken ct)
        {
            if (id != request.Id)
            {
                return BadRequest("Route id and payload id must match.");
            }

            try
            {
                var result = await _update.ExecuteAsync(request, ct);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            var result = await _delete.ExecuteAsync(id, ct);
            if (!result.Success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}


