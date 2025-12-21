using GymMangmerDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        IEnumerable<Plan> GetAll();
        Plan? GetById(int id);
        int Update(Plan plan);
    }
}
