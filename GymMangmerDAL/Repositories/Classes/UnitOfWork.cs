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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly GymDBContext _dBContext;

        public UnitOfWork(GymDBContext dBContext, ISessionRepository sessionRepository) 
        {
            _dBContext = dBContext;
            SessionRepository = sessionRepository;
        }

        public ISessionRepository SessionRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            //TEntity == member
            var EntityType = typeof(TEntity);
            if(_repositories.TryGetValue(EntityType,out var repo))
                return (IGenericRepository<TEntity>)repo;
            var NewRepo = new GenericRepository<TEntity>(_dBContext);
            _repositories[EntityType] = NewRepo;
            return NewRepo;
        }

        public int SaveChanges()
        {
            return _dBContext.SaveChanges();    
        }
    }
}
