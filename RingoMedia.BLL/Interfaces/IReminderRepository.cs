using RingoMedia.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingoMedia.BLL.Interfaces
{
    public interface IReminderRepository
    {
        Task<Reminder> GetByIdAsync(int id);
        Task<IEnumerable<Reminder>> GetAllAsync();
        Task AddAsync(Reminder reminder);
        Task UpdateAsync(Reminder reminder);
        Task DeleteAsync(int id);
    }
}
