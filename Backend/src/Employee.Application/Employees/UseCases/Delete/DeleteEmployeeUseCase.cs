using System.Threading;
using System.Threading.Tasks;
using EmployeeManagerDomain.Employees;

namespace EmployeeManagement.Application.Employees.UseCases.Delete
{
    public sealed class DeleteEmployeeUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public DeleteEmployeeUseCase(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<DeleteEmployeeResponse> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository.GetByIdAsync(id, tracking: true, cancellationToken);
            if (employee is null)
            {
                return new DeleteEmployeeResponse(false);
            }

            _employeeRepository.Delete(employee);
            await _employeeRepository.SaveChangesAsync(cancellationToken);

            return new DeleteEmployeeResponse(true);
        }
    }
}


