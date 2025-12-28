using GymMangmentBLL.ViewModels.SessionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentBLL.Service.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel GetSessionById(int Sessionid);
        bool CreateSession(CreateSessionViewModel createSession);
        UpdateSessionViewModel? GetSessionToUpdate(int sessionid);
        bool UpdateSession(int sessionid,UpdateSessionViewModel updateSession);
        bool RemoveSession(int sessionid);
    }
}
