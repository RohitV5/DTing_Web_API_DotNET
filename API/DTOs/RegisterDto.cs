using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }

        //Different validators can be added in property so that when recived from client these are validated
        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
    }
}