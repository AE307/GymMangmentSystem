using GymMangmentBLL.Service.Interfaces;
using GymMangmentBLL.ViewModels.AnalyticsViewModel;
using GymMangmerDAL.Entities;
using GymMangmerDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentBLL.Service.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public AnalyticsViewModel GetAnalyticsData()
        {
            var Sessions = _unitOfWork.GetRepository<Session>().GetAll();
            return new AnalyticsViewModel
            {
                ActiveMembers = _unitOfWork.GetRepository<MemberShip>().GetAll(x=>x.Status=="Active").Count(),
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                TotalTrainer = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = Sessions.Where(x=>x.StartDate>DateTime.Now).Count(),
                OngoingSession = Sessions.Where(x => x.StartDate <= DateTime.Now && x.EndDate>=DateTime.Now).Count(),
                CompletedSession = Sessions.Where(x => x.EndDate < DateTime.Now).Count()
            };
        }
    }
}
