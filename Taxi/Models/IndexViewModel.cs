using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.ViewModels;
using Taxi.ViewModelss;

namespace Taxi.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Calls> Calls { get; set; }
        public IEnumerable<Cars> Cars { get; set; }
        public IEnumerable<Associates> Associates { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterView FilterView { get; set; }
    }
}
