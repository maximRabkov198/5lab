using System;
using System.Collections.Generic;

namespace Taxi.Models
{
    public partial class Rates
    {
        public Rates()
        {
            Calls = new HashSet<Calls>();
        }

        public int Ratesid { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }

        public ICollection<Calls> Calls { get; set; }
    }
}
