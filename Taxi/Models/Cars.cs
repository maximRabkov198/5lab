using System;
using System.Collections.Generic;

namespace Taxi.Models
{
    public partial class Cars
    {
        public Cars()
        {
            Calls = new HashSet<Calls>();
        }

        public int Carsid { get; set; }
        public int RegistrationNumber { get; set; }
        public int MarkId { get; set; }
        public int BodyNumber { get; set; }
        public DateTime Data { get; set; }
        public int EngineNumber { get; set; }
        public DateTime Year { get; set; }
        public int Mileage { get; set; }
        public DateTime DateOfLastMaintetance { get; set; }
        public string Machanic { get; set; }
        public string SpecialMarks { get; set; }
        public int DriverId { get; set; }

        public Associates Driver { get; set; }
        public CarsMark Mark { get; set; }
        public ICollection<Calls> Calls { get; set; }
    }
}
