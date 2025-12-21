using GymMangmerDAL.Data.Context;
using GymMangmerDAL.Entities;
using GymMangmerDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDBContext _dBContext;

        public SessionRepository(GymDBContext dBContext) : base(dBContext)
        {
            _dBContext = dBContext;
        }
        public IEnumerable<Session> GetAllSessionWithTrainerAndCategory()
        {
            return _dBContext.Sessions.Include(x => x.SessionTrainer).Include(x => x.SessionCategory).ToList();
        }

        public int GetCountOfBookedSlot(int sessionId)
        {
            return  _dBContext.MemberSessions.Where(x=>x.SessionId==sessionId).Count();
        }
    }
}
