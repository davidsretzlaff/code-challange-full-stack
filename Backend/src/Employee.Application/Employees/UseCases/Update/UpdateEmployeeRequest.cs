namespace EmployeeManagement.Application.Employees.UseCases.Update
{
    public sealed record UpdateEmployeeRequest(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string Position
    );
}


