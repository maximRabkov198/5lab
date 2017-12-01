using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taxi.ViewModelss
{

    public class FilterView
    {
        public FilterView(string Name, int? Experiance, int Age,string Machanic, int RegistrationNumber,int BodyNumber,int PhoneNumber,string DstinationNumber)
        {
            SelectedName = Name;
            SelectedExperiance = Experiance;
            SelectedAge = Age;
            SelectedMachanic = Machanic;
            SelectedRegistrationNumber = RegistrationNumber;
            SelectedBodyNumber = BodyNumber;
            SelectedPhoneNumber = PhoneNumber;
            SelectedDstinationNumber = DstinationNumber;
        }

        public string SelectedName { get; set; }
        public int? SelectedExperiance { get; set; }
        public int SelectedAge { get; set; }
        public string SelectedNameClient { get; set; }
        public string SelectedMachanic { get; set; }
        public int SelectedRegistrationNumber { get; set; }
        public int SelectedBodyNumber { get; set; }
        public int SelectedPhoneNumber { get; set; }
        public string SelectedDstinationNumber { get; set; }
    }

}
