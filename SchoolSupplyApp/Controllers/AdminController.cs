using SchoolSupplyApp.Data;
using SchoolSupplyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolSupplyApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Supplies()
        {
            var vm = new SuppliesViewModel();
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            vm.Supplies = repo.GetAllSupplies();
            return View(vm);
        }
        public ActionResult AddSupply(Supply s)
        {
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            repo.AddSupply(s);
            return Redirect("/admin/supplies");
        }
        public ActionResult UpdateSupply(Supply s)
        {
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            repo.UpdateSupply(s);
            return Redirect("/admin/supplies");
        } 
        public ActionResult GetSupplies()
        {
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            var supplies = repo.GetAllSupplies().Select(s => new
            {
                id = s.Id,
                name = s.Name
            });
            return Json(supplies, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Lists()
        {
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            var accountRepo = new AccountRepository(Properties.Settings.Default.ConStr);
            var vm = new ListsViewModel();
            vm.Lists = repo.GetLists((accountRepo.GetSchool(User.Identity.Name)).Id);
            return View(vm);
        }
        public ActionResult List(int id)
        {
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            var vm = new ListViewModel();
            vm.Supplies = repo.GetListOfSupplies(id);
            vm.List = repo.GetList(id);
            if(TempData["Message"] != null)
            {
                vm.Message = (string)TempData["Message"];
            }
            return View(vm);
        }
        public ActionResult CreateList()
        {
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            var accountRepo = new AccountRepository(Properties.Settings.Default.ConStr);
            var vm = new CreateListViewModel();
            vm.School = accountRepo.GetSchool(User.Identity.Name);
            vm.Supplies = repo.GetAllSupplies();
            return View(vm);
        }
        [HttpPost]
        public ActionResult AddList(List l, IEnumerable<ListSupply> listSupplies)
        {
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            repo.AddList(l, listSupplies);
            return Redirect("/admin/lists");
        }
        [HttpPost]
        public ActionResult AddListSupply(ListSupply ls)
        {
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            bool wasSuccessfullyAdded = repo.AddSupplyToList(ls);
            if (!wasSuccessfullyAdded)
            {
                TempData["Message"] = "Sorry, the supply you want to add is already on this list.  You can update the quantity if you'd like by clicking the \"edit\" button.";
            }
            return Redirect("/admin/list?id=" + ls.ListId);
        }
        [HttpPost]
        public void RemoveListSupply(int listId, int supplyId)
        {
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            repo.RemoveSupplyFromList(listId, supplyId);
        }
        [HttpPost]
        public ActionResult UpdateListSupply(ListSupply ls)
        {
            var repo = new SchoolSupplyRepository(Properties.Settings.Default.ConStr);
            repo.UpdateListSupply(ls);
            return Redirect("/admin/list?id=" + ls.ListId);
        }
    }
}
