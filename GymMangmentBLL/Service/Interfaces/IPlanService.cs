using GymMangmentBLL.ViewModels.PlanViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentBLL.Service.Interfaces
{
    internal interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanById(int planId);
        UpdatePlanViewModel? GetPlanToUpdate(int planId);
        bool UpdatePlan(int planId, UpdatePlanViewModel planToUpdate);
        bool toggleStatus(int planId);
    }
}
