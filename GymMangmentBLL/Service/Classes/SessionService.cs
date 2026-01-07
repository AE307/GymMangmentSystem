using AutoMapper;
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
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createSession)
        {
            try
            {
                if (!IsTrainerExists(createSession.TrainerId)) return false;
                if (!IsCategoryExists(createSession.CategoryId)) return false;
                if (!IsDateTimeValid(createSession.StartDate, createSession.EndDate)) return false;
                if (createSession.Capacity > 25 || createSession.Capacity < 0) return false;
                var sessionEntity = _mapper.Map<Session>(createSession);
                _unitOfWork.GetRepository<Session>().Add(sessionEntity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Create Session Failed {ex}");
                return false;
            }
           
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
           var Sessions =  _unitOfWork.SessionRepository.GetAllSessionWithTrainerAndCategory();
            if (Sessions is null || !Sessions.Any()) return [];
            var mappedSession = _mapper.Map<IEnumerable<SessionViewModel>>(Sessions);
            foreach(var session in mappedSession)
            {
                session.AvailableSpots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlot(session.Id);
            }
            return mappedSession;
            //return Sessions.Select(s => new SessionViewModel
            //{
            //    Id = s.Id,
            //    Description = s.Description,
            //    StartDate = s.StartDate,
            //    EndDate = s.EndDate,
            //    Capacity = s.Capacity,
            //    TrainerName = s.SessionTrainer.Name,
            //    CategoryName = s.SessionCategory.CategoryName.ToString(),
            //    AvailableSpots = s.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlot(s.Id)
            //});
        }

        public IEnumerable<TrainerSelectViewModel> GetTrainerForDropDown()
        {
           var trainer= _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainer);
        }

        public IEnumerable<CategorySelectViewModel> GetCategoryForDropDown()
        {
            var category = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(category);
        }

        public SessionViewModel GetSessionById(int Sessionid)
        {
            var session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(Sessionid);
            if (session is null) return null;
            var mappedSession = _mapper.Map<SessionViewModel>(session);
            mappedSession.AvailableSpots = mappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlot(session.Id);
            return mappedSession;
            //return new SessionViewModel
            //{
            //    Description = session.Description,
            //    StartDate = session.StartDate,
            //    EndDate = session.EndDate,
            //    TrainerName = session.SessionTrainer.Name,
            //    CategoryName = session.SessionCategory.CategoryName.ToString(),
            //    AvailableSpots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlot(session.Id)
            //};
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionid)
        {
            var Session = _unitOfWork.SessionRepository.GetById(sessionid);
            if(!IsSessionAvailableToUpdate(Session!))return null;
            return _mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(int sessionid, UpdateSessionViewModel updateSession)
        {
            try
            {
                var Session=  _unitOfWork.SessionRepository.GetById(sessionid);
                if (!IsSessionAvailableToUpdate(Session!)) return false;
                if (!IsTrainerExists(updateSession.TrainerId)) return false;
                if (!IsDateTimeValid(updateSession.StartDate, updateSession.EndDate)) return false;
                _mapper.Map(updateSession, Session);
                Session!.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<Session>().Update(Session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Session Failed {ex}");
                return false;
            }
        }

        public bool RemoveSession(int sessionid)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionid);
                if (!IsSessionAvailaleForRemovig(Session!)) return false;
                _unitOfWork.SessionRepository.Delete(Session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Remove Session Failed {ex}");
                return false;
            }
        }

        #region Helper Methods
        private bool IsTrainerExists(int Trainerid)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(Trainerid) is not null;
        }
        private bool IsCategoryExists(int Categoryid)
        { 
            return _unitOfWork.GetRepository<Category>().GetById(Categoryid) is not null;
        }
        private bool IsDateTimeValid(DateTime Startdate,DateTime Enddate)
        {
            return Startdate < Enddate;
        }
        private bool IsSessionAvailableToUpdate(Session session)
        {
            if (session is null) return false;
            ///if  session complete
            if (session.EndDate<DateTime.Now) return false;
            // if session started
            if (session.StartDate<= DateTime.Now) return false;
            //if session has active booking
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlot(session.Id) > 0;
            if (HasActiveBooking) return false;
            return true;

        }
        private bool IsSessionAvailaleForRemovig(Session session) 
        { 
            if (session is null) return false;
            ///if  session started
            if (session.StartDate<=DateTime.Now && session.EndDate > DateTime.Now) return false;
            // if session is upcoming
            if (session.StartDate > DateTime.Now) return false;
            // if session has active booking
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlot(session.Id) > 0;
            if (HasActiveBooking) return false;
            return true;
        }

        #endregion
    }
}
