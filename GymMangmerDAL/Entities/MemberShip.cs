using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Entities
{
    public class MemberShip: BaseEntity
    {
        // strartDate == createdat of base entity
        public DateTime EndDate { get; set; }
        public string Status { 
            get 
            { 
             if (DateTime.Now > EndDate)
                return "Expired";
             else
                return "Active";

            }
        }
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
        public int PlanId { get; set; }
        public Plan Plan { get; set; }=null!;
    }
}
