using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RingoMedia.BLL.Interfaces;
using RingoMedia.DAL.Models;
using RingoMedia.PL.ViewModels;

namespace RingoMedia.PL.Controllers
{
    public class RemindersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RemindersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {

            var reminders = await _unitOfWork.Reminders.GetAllAsync();
            var reminderViewModels = _mapper.Map<IEnumerable<ReminderViewModel>>(reminders);
            return View(reminderViewModels);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReminderViewModel reminderViewModel)
        {
            if (ModelState.IsValid)
            {
                var reminder = _mapper.Map<Reminder>(reminderViewModel);
                await _unitOfWork.Reminders.AddAsync(reminder);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reminderViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reminder = await _unitOfWork.Reminders.GetByIdAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }

            var reminderViewModel = _mapper.Map<ReminderViewModel>(reminder);
            return View(reminderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReminderViewModel reminderViewModel)
        {
            if (id != reminderViewModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var reminder = await _unitOfWork.Reminders.GetByIdAsync(id);
                if (reminder == null)
                {
                    return NotFound();
                }

                _mapper.Map(reminderViewModel, reminder);
                await _unitOfWork.Reminders.UpdateAsync(reminder);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reminderViewModel);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var reminder = await _unitOfWork.Reminders.GetByIdAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }
            var reminderViewModel = _mapper.Map<ReminderViewModel>(reminder);
            return View(reminderViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Reminders.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
