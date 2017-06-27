using SchoolSupplyApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolSupplyApp.Models
{
    public class ViewListViewModel
    {
        public Family Family { get; set; }
        public IEnumerable<SupplyWithQuantity> Supplies { get; set; }
    }
}