using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC_Console
{
    public class RBCDataViewModel
    {
        public string? Title { get; set; }
        public string? Address { get; set; }
        public string BranchId { get; set; }
        public List<EmployeeViewModel> Emaployees { get; set; }
    }
}
