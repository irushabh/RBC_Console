using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace RBC_Console
{
    public class AdvisorResponseViewModel
    {
        public bool success { get; set; }
        public data data { get; set; }
    }

    public class data
    {
        public string? html { get; set; }
        public int count { get; set; }
    }
}
