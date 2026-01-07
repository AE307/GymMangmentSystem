using GymMangmentBLL.ViewModels.PlanViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentBLL.Service.Interfaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();

        PlanViewModel? GetPlaneById(int planId);

        UpdatePlanViewModel? GetPlaneToUpdate(int planId);

        bool UpdatePlan(int planId, UpdatePlanViewModel updatedPlan);
        bool ToggleStatus(int planId);
    }
}
