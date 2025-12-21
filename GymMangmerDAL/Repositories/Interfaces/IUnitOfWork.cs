using GymMangmerDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public ISessionRepository SessionRepository { get; }
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        int SaveChanges();
    }
}
