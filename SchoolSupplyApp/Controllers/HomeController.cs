using SchoolSupplyApp.Data;
using SchoolSupplyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolSupplyApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/admin/index");
            }
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            var vm = new IndexViewModel();
            vm.Schools = repo.GetAllSchools();
            return View(vm);
        }
        public ActionResult GetSchools()
        {
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            var schools = repo.GetAllSchools().Select(s => new
            {
                id = s.Id,
                name = s.Name
            });
            return Json(schools, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddFamily(Family f, IEnumerable<Children> children)
        {
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            repo.AddFamilyWithChildren(f, children);
            Response.Cookies.Add(new HttpCookie("FamilyId", f.Id.ToString()));
            return Redirect("/home/viewlist?familyid=" + f.Id);
        }
        public ActionResult ViewList(int? familyId)
        {
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            var vm = new ViewListViewModel();
            if(familyId != null)
            {
                vm.Supplies = repo.GetTotalSupplyList(familyId.Value);
                vm.Family = repo.GetFamily(familyId.Value);
            }
            else if(Request.Cookies["FamilyId"] != null)
            {
                vm.Supplies = repo.GetTotalSupplyList(int.Parse(Request["FamilyId"]));
                vm.Family = repo.GetFamily(int.Parse(Request["FamilyId"]));
            }
            else
            {
                return Redirect("/");
            }
            return View(vm);
        }
        public ActionResult GetGradesForSchool(int schoolId)
        {
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            var grades = repo.GetGradesForSchool(schoolId);
            return Json(new { grades = grades}, JsonRequestBehavior.AllowGet);
        }

    }
}