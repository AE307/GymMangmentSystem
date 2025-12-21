using GymMangmerDAL.Data.Context;
using GymMangmerDAL.Entities;
using GymMangmerDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository 
    {
        private readonly GymDBContext _dBContext;

        public PlanRepository(GymDBContext dBContext) 
        {
            _dBContext = dBContext;
        }
        public IEnumerable<Plan> GetAll()=> _dBContext.Plans.ToList();


        public Plan? GetById(int id)=> _dBContext.Plans.Find(id);


        public int Update(Plan plan)
        {
            _dBContext.Plans.Update(plan);
            return _dBContext.SaveChanges();
        }
    }
}
