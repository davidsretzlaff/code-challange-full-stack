using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EmployeeManagement.Application.Employees.DTOs;
using EmployeeManagerDomain.Employees;

namespace EmployeeManagement.Application.Employees.UseCases.Update
{
    public sealed class UpdateEmployeeUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public UpdateEmployeeUseCase(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<UpdateEmployeeResponse> ExecuteAsync(UpdateEmployeeRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(request.Id), "Id must be greater than zero.");
            }

            var employee = await _employeeRepository.GetByIdAsync(request.Id, tracking: true, cancellationToken);
            if (employee is null)
            {
                throw new KeyNotFoundException($"Employee with id {request.Id} was not found.");
            }

            employee.Update(request.FirstName, request.LastName, request.Email, request.Position);

            _employeeRepository.Update(employee);
            await _employeeRepository.SaveChangesAsync(cancellationToken);

            var dto = EmployeeMapper.ToDto(employee);
            return new UpdateEmployeeResponse(dto);
        }
    }
}


