using System;
using System.Collections.Generic;

namespace Taxi.Models
{
    public partial class Associates
    {
        public Associates()
        {
            Cars = new HashSet<Cars>();
        }

        public int Associatesid { get; set; }
        public int? Experiance { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public int PostId { get; set; }

        public Post Post { get; set; }
        public ICollection<Cars> Cars { get; set; }
    }
}
