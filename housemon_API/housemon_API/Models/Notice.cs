using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace housemon_API.Models
{
    public class Notice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string noticeId { get; set; }
        public string userId { get; set; }
        public string propertyId { get; set; }
        public DateTimeOffset startDate  = DateTimeOffset.Now;
        public DateTimeOffset endDate { get; set;} 
        public string data { get; set; }

    }
}
