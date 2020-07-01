using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTOs.AuthorDTOs
{
    public class CreateAuthorDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]

        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string MainCategory { get; set; }
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }
    }
}
