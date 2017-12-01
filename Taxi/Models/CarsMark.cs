using System;
using System.Collections.Generic;

namespace Taxi.Models
{
    public partial class CarsMark
    {
        public CarsMark()
        {
            Cars = new HashSet<Cars>();
        }

        public int CarsMarkid { get; set; }
        public string Specifications { get; set; }
        public int Cost { get; set; }
        public string Specificity { get; set; }

        public ICollection<Cars> Cars { get; set; }
    }
}
