using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Entities
{
    public class Member: GymUser
    {
        //join date == createdat of base entity
        public string? photo { get; set; }
        #region HealthRecord
        public HealthRecord HealthRecord { get; set; } = null!;
        #endregion

        #region Member-MemberShip
        public ICollection<MemberShip> MemberShips { get; set; } = null!;
        #endregion

        #region Member-MemberSession
        public ICollection<MemberSession> MemberSessions { get; set; } = null!;
        #endregion
    }
}
