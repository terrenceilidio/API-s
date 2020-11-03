using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace housemon_API.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string userId { get; set; }  
  
        public string names { get; set; }
   
        public string gender { get; set; }
        //public List<string> tokens{ get; set; }

        public string idNumber { get; set; }   
  
        public string username { get; set; }
     
        public string cellNumber { get; set; } 
  
        public string password { get; set; }
        public string role { get; set; }
        public DateTimeOffset createdAt { get; set; } = DateTime.UtcNow;
        public DateTimeOffset? updatedAt { get; set; } 
        public string? profilePicture  { get; set; }
    }

}
