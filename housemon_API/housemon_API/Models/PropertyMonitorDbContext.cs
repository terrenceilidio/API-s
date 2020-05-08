using Microsoft.EntityFrameworkCore;
using housemon_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace housemon_API.Models
{
    public class PropertyMonitorDbContext: DbContext
    {
        public PropertyMonitorDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Manager> managers { get; set; }
        public DbSet<Tenant> tenants { get; set; }
        public DbSet<Property> properties { get; set; }
        public DbSet<Landlord> landlords { get; set; }
        public DbSet<Lease> leases { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Complaint> complaints { get; set; }
        public DbSet<LandLordProperty> landlordProperties { get; set; }

    }

  
}
