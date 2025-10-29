namespace EmployeeManagement.Application.Employees.DTOs
{
    public sealed record EmployeeDto(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string Position
    );
}


