using System.Threading;
using System.Threading.Tasks;
using EmployeeManagement.Application.Employees.DTOs;
using EmployeeManagerDomain.Employees;

namespace EmployeeManagement.Application.Employees.UseCases.GetById
{
    public sealed class GetEmployeeByIdUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeeByIdUseCase(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<GetEmployeeByIdResponse> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository.GetByIdAsync(id, tracking: false, cancellationToken);
            var dto = employee is null ? null : EmployeeMapper.ToDto(employee);
            return new GetEmployeeByIdResponse(dto);
        }
    }
}


