using System;
using System.Collections.Generic;

namespace Taxi.Models
{
    public partial class Calls
    {
        public int Callsid { get; set; }
        public DateTime DateAndTime { get; set; }
        public int PhoneNumber { get; set; }
        public string DstinationNumber { get; set; }
        public int CarId { get; set; }

        public Cars Car { get; set; }
        public Rates Rate { get; set; }
    }
}
