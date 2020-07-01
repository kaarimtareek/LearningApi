using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.AuthorDTOs
{
    class CreateAuthorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Category { get; set; }
        public string MainCategory { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
    }
}
