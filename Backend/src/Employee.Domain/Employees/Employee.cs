using EmployeeManagerDomain.Common;

namespace EmployeeManagerDomain.Employees
{
    public sealed class Employee
    {
        public Employee(string firstName, string lastName, string email, string position)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Position = position;
        }

        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; } 
        public string Position { get; private set; }

        public static Employee Create(string firstName, string lastName, string email, string position) 
        {
            var validFirstName = DomainValidate.Name(firstName, nameof(firstName));
            var validLastName = DomainValidate.Name(lastName, nameof(lastName));
            var validEmail = DomainValidate.Email(email);
            var validPosition = DomainValidate.Position(position);

            var employee = new Employee(validFirstName, validLastName, validEmail, validPosition);
            return employee;
        }

        public void Update(string firstName, string lastName, string email, string position)
        {
            var validFirstName = DomainValidate.Name(firstName, nameof(firstName));
            var validLastName = DomainValidate.Name(lastName, nameof(lastName));
            var validEmail = DomainValidate.Email(email);
            var validPosition = DomainValidate.Position(position);

            FirstName = validFirstName;
            LastName = validLastName;
            Email = validEmail;
            Position = validPosition;
        }
    }
}
