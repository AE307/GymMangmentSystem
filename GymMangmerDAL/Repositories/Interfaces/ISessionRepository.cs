using GymMangmerDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Repositories.Interfaces
{
    public interface ISessionRepository:IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionWithTrainerAndCategory();
        int GetCountOfBookedSlot(int sessionId);

    }
}
