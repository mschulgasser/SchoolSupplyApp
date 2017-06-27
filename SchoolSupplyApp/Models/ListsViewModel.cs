using SchoolSupplyApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolSupplyApp.Models
{
    public class ListsViewModel
    {
        public IEnumerable<List> Lists { get; set; }
    }
}