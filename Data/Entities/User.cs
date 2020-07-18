using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Index("IX_USER_EMAIL",IsUnique =true)]
        public string Email { get; set; }
        [MaxLength(200)]
        [Required]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; } 
        [Required]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        public string CountryIso { get; set; }
        [ForeignKey("CountryIso")]
        public Country Country { get; set; }
        [Required]
        public string Salt { get; set; }
        

    }
}
