using GymMangmerDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        //getall
        IEnumerable<TEntity> GetAll(Func<TEntity,bool>? condition=null);
        //getbyid
        TEntity? GetById(int id);
        //add
        void Add(TEntity entity);
        //update
        void Update(TEntity entity);
        //delete
        void Delete(TEntity entity);
    }
}
