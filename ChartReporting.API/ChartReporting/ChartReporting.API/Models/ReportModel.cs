using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChartReporting.API.Models
{
    public class Report
    {
        public Report()
        {
            Title = "Energy report";
            Description = "Tips to low your energy expenses";
        }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}