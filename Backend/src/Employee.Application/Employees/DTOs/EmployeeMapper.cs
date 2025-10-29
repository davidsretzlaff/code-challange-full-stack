using EmployeeManagerDomain.Employees;

namespace EmployeeManagement.Application.Employees.DTOs
{
    internal static class EmployeeMapper
    {
        public static EmployeeDto ToDto(Employee employee)
        {
            return new EmployeeDto(
                employee.Id,
                employee.FirstName,
                employee.LastName,
                employee.Email,
                employee.Position
            );
        }
    }
}


