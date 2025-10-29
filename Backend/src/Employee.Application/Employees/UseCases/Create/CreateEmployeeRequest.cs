namespace EmployeeManagement.Application.Employees.UseCases.Create
{
    public sealed record CreateEmployeeRequest(
        string FirstName,
        string LastName,
        string Email,
        string Position
    );
}


