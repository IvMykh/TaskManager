using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManager.WebUI.Models
{
    public class DaySummaryItemModel
    {
        public string Caption { get; set; }
        public int Quantity { get; set; }
        public string StyleClass { get; set; }
    }
}