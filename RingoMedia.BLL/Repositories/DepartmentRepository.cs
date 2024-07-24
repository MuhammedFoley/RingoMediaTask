using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RingoMedia.BLL.Interfaces;
using RingoMedia.DAL.Contextes;
using RingoMedia.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingoMedia.BLL.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private const string DepartmentCacheKey = "all_departments";

        public DepartmentRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            if (!_cache.TryGetValue(DepartmentCacheKey, out IEnumerable<Department> departments))
            {
                departments = await _context.Departments
                    .Include(d => d.SubDepartments)
                    .ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _cache.Set(DepartmentCacheKey, departments, cacheOptions);
            }

            return departments;
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.SubDepartments)
                .Include(d => d.ParentDepartment)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddDepartmentAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
            _cache.Remove(DepartmentCacheKey); // Invalidate cache
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            _context.Departments.Update(department);
            _cache.Remove(DepartmentCacheKey); // Invalidate cache
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                _cache.Remove(DepartmentCacheKey); // Invalidate cache
            }
        }
    }



}
