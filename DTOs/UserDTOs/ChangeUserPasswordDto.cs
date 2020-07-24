using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.UserDTOs
{
    public class ChangeUserPasswordDto
    {
        public Guid Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
