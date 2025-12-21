using GymMangmentBLL.Service.Interfaces;
using GymMangmentBLL.ViewModels.SessionViewModel;
using GymMangmerDAL.Entities;
using GymMangmerDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentBLL.Service.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<SessionViewModel> GetAllSessions()
        {
           var Sessions =  _unitOfWork.SessionRepository.GetAllSessionWithTrainerAndCategory();
            if (Sessions is null || !Sessions.Any()) return [];
            return Sessions.Select(s => new SessionViewModel
            {
                Id = s.Id,
                Description = s.Description,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Capacity = s.Capacity,
                TrainerName = s.SessionTrainer.Name,
                CategoryName = s.SessionCategory.CategoryName.ToString(),
                AvailableSpots = s.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlot(s.Id)
            });
        }
    }
}
