using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC_Console
{
    public class EventLog
    {
        public DateTime EventDate { get; set; }
        public String EventName { get; set; }
        public String EventLocation { get; set; }
        public override string ToString()
        {
            return EventDate + " " + EventName + " " + EventLocation;
        }
    }
}
