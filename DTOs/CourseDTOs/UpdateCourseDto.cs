using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTOs.CourseDTOs
{
    public class UpdateCourseDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid AuthorId { get; set; }
    }
}
