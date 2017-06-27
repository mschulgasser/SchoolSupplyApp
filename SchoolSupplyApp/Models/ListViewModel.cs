using SchoolSupplyApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolSupplyApp.Models
{
    public class ListViewModel
    {
        public List List { get; set; }
        public IEnumerable<ListSupply> Supplies { get; set; }
        public string Message { get; set; }
    }
}