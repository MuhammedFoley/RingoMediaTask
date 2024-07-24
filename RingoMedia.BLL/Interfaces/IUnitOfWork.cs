using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingoMedia.BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDepartmentRepository Departments { get; }
        IReminderRepository Reminders { get; }
        Task<int> CompleteAsync();
    }

}
