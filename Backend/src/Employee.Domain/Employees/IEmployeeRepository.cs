namespace EmployeeManagerDomain.Employees
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
        Task<Employee?> GetByIdAsync(int id, bool tracking, CancellationToken cancellationToken = default);
        Task<List<Employee>> ListAsync(CancellationToken cancellationToken = default);
        void Update(Employee employee);
        void Delete(Employee employee);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}


