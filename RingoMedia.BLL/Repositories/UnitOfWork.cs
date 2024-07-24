using Microsoft.Extensions.Caching.Memory;
using RingoMedia.BLL.Interfaces;
using RingoMedia.DAL.Contextes;


namespace RingoMedia.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private IDepartmentRepository _departmentRepository;
        private IReminderRepository _reminderRepository;

        public UnitOfWork(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public IDepartmentRepository Departments => _departmentRepository ??= new DepartmentRepository(_context, _cache);
        public IReminderRepository Reminders
        {
            get
            {
                return _reminderRepository ??= new ReminderRepository(_context);
            }
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }


}
