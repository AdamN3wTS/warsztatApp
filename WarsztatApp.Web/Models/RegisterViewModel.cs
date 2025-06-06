﻿using System.ComponentModel.DataAnnotations;

namespace WarsztatApp.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Hasło musi się zgadzać")]
        public string ConfirmPassword { get; set; }
    }
}
