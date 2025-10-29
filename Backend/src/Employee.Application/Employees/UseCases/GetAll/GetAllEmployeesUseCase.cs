using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmployeeManagement.Application.Employees.DTOs;
using EmployeeManagerDomain.Employees;

namespace EmployeeManagement.Application.Employees.UseCases.GetAll
{
    public sealed class GetAllEmployeesUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetAllEmployeesUseCase(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<GetAllEmployeesResponse> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var employees = await _employeeRepository.ListAsync(cancellationToken);
            var items = employees.Select(EmployeeMapper.ToDto).ToList();
            return new GetAllEmployeesResponse(items);
        }
    }
}



