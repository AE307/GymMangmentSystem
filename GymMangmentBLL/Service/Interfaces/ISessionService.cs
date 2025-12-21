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
    }
}
