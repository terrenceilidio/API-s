using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace housemon_API.Models
{
    public class Landlord: User
    {
        public virtual ICollection<LandLordProperty> landlordProperties { get; set; } = new List<LandLordProperty>();
    }
}