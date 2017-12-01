using System;
using System.Collections.Generic;

namespace Taxi.Models
{
    public partial class Post
    {
        public Post()
        {
            Associates = new HashSet<Associates>();
        }

        public int Postid { get; set; }
        public string Name { get; set; }

        public ICollection<Associates> Associates { get; set; }
    }
}
