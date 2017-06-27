using SchoolSupplyApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolSupplyApp.Models
{
    public class CreateListViewModel
    {
        public School School { get; set; }
        public IEnumerable<Supply> Supplies { get; set; }
    }
}