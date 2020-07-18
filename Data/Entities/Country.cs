using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;

namespace Data.Entities
{
    public class Country
    {
        [Key]
        public string Iso{ get; set; }
        [Index("IX_COUNTRY_PHONE_CODE",IsUnique =true)]
        public string PhoneCode { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();

    }
}
