using System.Collections.Generic;
using EmployeeManagement.Application.Employees.DTOs;

namespace EmployeeManagement.Application.Employees.UseCases.GetAll
{
    public sealed record GetAllEmployeesResponse(List<EmployeeDto> Employees);
}



