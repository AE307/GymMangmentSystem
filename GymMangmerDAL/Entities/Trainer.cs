using GymMangmerDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Entities
{
    public class Trainer:GymUser
    {
        //hiringdate == createdat of base entity
        public Specialties Specialties { get; set; }
        #region Trainer-Session
        public ICollection<Session> TrainerSession { get; set; } = null!;
        #endregion
    }
}
