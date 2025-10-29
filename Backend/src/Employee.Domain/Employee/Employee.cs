namespace Employee.Domain.Employee
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
            // validations
            var employee = new Employee(firstName, lastName, email, position);
            return employee;
        }

    }
}
