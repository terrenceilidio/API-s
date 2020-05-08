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
        [Required]
        [MaxLength(100)]
        public string names { get; set; }
        [MaxLength(6)]
        public string gender { get; set; }
        [MaxLength(13)]
        public string idNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string username { get; set; }
        [MaxLength(20)]
        public string cellNumber { get; set; }
        [Required]
        [MaxLength(16)]
        public string password { get; set; }
        [Required]
        public string role { get; set; }
        public DateTime createdAt { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        public DateTime updatedAt { get; set; } 
        public string profilePicture { get; set; }

        //we  will use this to cast a userType to a user
        public T makeUser<T>() where T:User, new()
        {
            return new T
            {
                names = names,
                gender = gender,
                idNumber = idNumber,
                username = username,
                cellNumber = cellNumber,
                updatedAt = updatedAt,
                profilePicture = profilePicture,
                createdAt = createdAt,
                userId = userId,
                password = password,
                role = role
            };
        }
    }

    //Todo : hash the possword preferebly use bcrypt
    //Todo : create timestamps for when the object was created a
}
