using GymMangmentBLL.Service.Interfaces;
using GymMangmerDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GymMangmentPL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public HomeController(IAnalyticsService analyticsService) 
        {
            _analyticsService = analyticsService;
        }
       public ActionResult Index()
        {
            var data = _analyticsService.GetAnalyticsData();
            return View(data);
        }
    }
}
