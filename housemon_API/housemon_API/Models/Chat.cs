using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace housemon_API.Models
{
    public class Chat
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string chartId { get; set; }
        public string receiverId { get; set; }
        public string senderId {get; set;}
        public string message { get; set; }
        public DateTimeOffset timeSent { get; set; }
        public DateTimeOffset timeReceived { get; set; }
    }
}
