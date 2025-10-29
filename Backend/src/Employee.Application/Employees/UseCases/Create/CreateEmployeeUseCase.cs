using System.Threading;
using System.Threading.Tasks;
using EmployeeManagement.Application.Employees.DTOs;
using EmployeeManagerDomain.Employees;

namespace EmployeeManagement.Application.Employees.UseCases.Create
{
    public sealed class CreateEmployeeUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public CreateEmployeeUseCase(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<CreateEmployeeResponse> ExecuteAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default)
        {
            var employee = Employee.Create(request.FirstName, request.LastName, request.Email, request.Position);

            await _employeeRepository.AddAsync(employee, cancellationToken);
            await _employeeRepository.SaveChangesAsync(cancellationToken);

            var dto = EmployeeMapper.ToDto(employee);
            return new CreateEmployeeResponse(dto);
        }
    }
}


