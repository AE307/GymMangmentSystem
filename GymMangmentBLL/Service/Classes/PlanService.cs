using GymMangmentBLL.Service.Interfaces;
using GymMangmentBLL.ViewModels.PlanViewModel;
using GymMangmerDAL.Entities;
using GymMangmerDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentBLL.Service.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any()) return [];

            return plans.Select(p => new PlanViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                Price = p.Price,
                IsActive = p.IsActive

            });

        }

        public PlanViewModel? GetPlaneById(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null) return null;

            return new PlanViewModel()
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive

            };
        }

        public UpdatePlanViewModel? GetPlaneToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || HasActiveMemberShip(planId)) return null;
            return new UpdatePlanViewModel()
            {
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                PlanName = plan.Name,
                Price = plan.Price
            };
        }



        public bool UpdatePlan(int planId, UpdatePlanViewModel updatedPlan)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || HasActiveMemberShip(planId)) return false;
            try
            {
                (plan.Description, plan.Price, plan.DurationDays, plan.UpdatedAt) =
                (updatedPlan.Description, updatedPlan.Price, updatedPlan.DurationDays, DateTime.Now);
                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        //softDelete
        public bool ToggleStatus(int planId)
        {
            var repo = _unitOfWork.GetRepository<Plan>();
            var plan = repo.GetById(planId);
            if (plan is null || HasActiveMemberShip(planId)) return false;

            plan.IsActive = plan.IsActive == true ? false : true;
            plan.UpdatedAt = DateTime.Now;
            try
            {
                repo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        #region Helper
        private bool HasActiveMemberShip(int planId) 
        {
            var activemembership = _unitOfWork.GetRepository<MemberShip>().GetAll(x=>x.PlanId==planId && x.Status=="Active");
            return activemembership.Any();
        }
        #endregion
    }
}
