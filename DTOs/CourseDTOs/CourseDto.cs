using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.CourseDTOs
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
