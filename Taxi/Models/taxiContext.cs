using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Taxi.Models
{
    public partial class taxiContext : IdentityDbContext<User>
    {
        public taxiContext()
        {
        }

        public taxiContext(DbContextOptions<taxiContext> options): base(options)
        {
        }

        public virtual DbSet<Associates> Associates { get; set; }
        public virtual DbSet<Calls> Calls { get; set; }
        public virtual DbSet<Cars> Cars { get; set; }
        public virtual DbSet<CarsMark> CarsMark { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Rates> Rates { get; set; }

    }
}
