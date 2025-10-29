using EmployeeManagement.Application.Employees.DTOs;

namespace EmployeeManagement.Application.Employees.UseCases.GetById
{
    public sealed record GetEmployeeByIdResponse(EmployeeDto? Employee);
}


