using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTOs.CourseDTOs
{
    public class AddCourseDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public Guid AuthorId { get; set; }
    }
}
