using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmployeeManagement.Infrastructure;
using EmployeeManagerDomain.Employees;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagerInfrastructure.Repositories
{
    public sealed class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _dbContext;

        public EmployeeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            await _dbContext.Employees.AddAsync(employee, cancellationToken);
        }

        public async Task<Employee?> GetByIdAsync(int id, bool tracking, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Employees.AsQueryable();
            query = tracking ? query.AsTracking() : query.AsNoTracking();
            return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<List<Employee>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Employees
                .AsNoTracking()
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync(cancellationToken);
        }

        public void Update(Employee employee)
        {
            _dbContext.Employees.Update(employee);
        }

        public void Delete(Employee employee)
        {
            _dbContext.Employees.Remove(employee);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}


