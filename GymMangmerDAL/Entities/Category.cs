using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Entities
{
    public class Category: BaseEntity
    {
        public string CategoryName { get; set; } = null!;

        public ICollection<Session> Sessions { get; set; } = null!;

    }
}
