using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.App.WebApp.Areas.Admin.Models
{
    public class RegisterViewModel
    {

        [Key]
        [MaxLength(50)]
        [Required(ErrorMessage = "Enter Email ! ")]
        [EmailAddress(ErrorMessage = "Wrong email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Password ! ")]
        [DataType(DataType.Password)]
        [MaxLength(32, ErrorMessage = "Password only have 32 character")]
        [MinLength(8, ErrorMessage = "Password be at least 8 characters long")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password doesn't match ! ")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "This field is require ! ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "This field is require ! ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This field is require ! ")]
        public string LastName { get; set; }
    }
}
