using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSupplyApp.Data
{
    public class SchoolSupplyRepository
    {
        private string _connectionString;
        public SchoolSupplyRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddFamilyWithChildren(Family f, IEnumerable<Children> children)
        {
            using(var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                context.Families.InsertOnSubmit(f);
                context.SubmitChanges();
                foreach(Children c in children)
                {
                    c.FamilyId = f.Id;
                }
                context.Childrens.InsertAllOnSubmit(children);
                context.SubmitChanges();
            }
        }
        public void AddSupply(Supply s)
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                context.Supplies.InsertOnSubmit(s);
                context.SubmitChanges();
            }
        }
        public void AddList(List l, IEnumerable<ListSupply> listSupplies)
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                context.Lists.InsertOnSubmit(l);
                context.SubmitChanges();
                foreach(ListSupply ls in listSupplies)
                {
                    ls.ListId = l.Id;
                }
                context.SubmitChanges();
            }
        }
        public IEnumerable<Supply> GetAllSupplies()
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                return context.Supplies.OrderBy(s => s.Name).ToList();
            }
        }
        public IEnumerable<School> GetAllSchools()
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<School>(s => s.Lists);
                context.LoadOptions = loadOptions;
                return context.Schools.ToList();
            }
        }
        public IEnumerable<List> GetLists(int schoolId)
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<List>(l => l.School);
                context.LoadOptions = loadOptions;
                return context.Lists.Where(l => l.SchoolId == schoolId).ToList();
            }
        }
        public IEnumerable<ListSupply> GetListOfSupplies(int listId)
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<ListSupply>(ls => ls.Supply);
                loadOptions.LoadWith<List>(l => l.ListSupplies);
                loadOptions.LoadWith<List>(l => l.School);
                context.LoadOptions = loadOptions;
                return context.ListSupplies.Where(ls => ls.ListId == listId).ToList();
            }
        }
        public List GetList(int listId)
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<List>(l => l.School);
                context.LoadOptions = loadOptions;
                return context.Lists.FirstOrDefault(l => l.Id == listId);
            }
        }
        public Family GetFamily(int id)
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Family>(f => f.Childrens);
                context.LoadOptions = loadOptions;
                return context.Families.FirstOrDefault(f => f.Id == id);
            }
        }
        public void UpdateSupply(Supply s)
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE Supplies SET Name = {0} WHERE Id = {1}", s.Name, s.Id);
            }
        }
        public bool AddSupplyToList(ListSupply ls)
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                var existingLS = context.ListSupplies.FirstOrDefault(s => s.ListId == ls.ListId && s.SupplyId == ls.SupplyId);
                if(existingLS != null)
                {
                    return false;
                }
                context.ListSupplies.InsertOnSubmit(ls);
                context.SubmitChanges();
                return true;
            }
        }
        public void RemoveSupplyFromList(int listId, int supplyId)
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                context.ExecuteCommand("DELETE ListSupply WHERE ListId = {0} AND SupplyId = {1}", listId, supplyId);
            }
        }
        public void UpdateListSupply(ListSupply ls)
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE ListSupply SET Quantity = {0} WHERE ListId = {1} AND SupplyId = {2}", ls.Quantity, ls.ListId, ls.SupplyId);
            }
        }
        public IEnumerable<SupplyWithQuantity> GetTotalSupplyList(int familyId)
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                var children = context.Families.FirstOrDefault(f => f.Id == familyId).Childrens.ToList();
                var result = new List<SupplyWithQuantity>();
                foreach (Children c in children)
                {
                    var list = context.Lists.FirstOrDefault(l => l.SchoolId == c.SchoolId && l.Grade.Equals(c.Grade));
                    if(list == null)
                    {
                        continue;
                    }
                    foreach(ListSupply ls in list.ListSupplies)
                    {
                        var existingSupply = result.FirstOrDefault(s => s.Supply.Id == ls.SupplyId);
                        if(existingSupply != null)
                        {
                            existingSupply.Quantity++;
                        }
                        else
                        {
                            var supplyWithQuantity = new SupplyWithQuantity()
                            {
                                Supply = ls.Supply,
                                Quantity = ls.Quantity
                            };
                            result.Add(supplyWithQuantity);
                        }
                    }
                }
                return result.OrderBy(s => s.Supply.Name);
            }
        }
        public string[] GetGradesForSchool(int schoolId)
        {
            using (var context = new SchoolSupplyAppDataContext(_connectionString))
            {
                var result = context.Lists.Where(l => l.SchoolId == schoolId).OrderBy(l => l.Grade).Select(l => l.Grade).ToArray();
                return result;
            }
        }
    }
}
