using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.App.WebApp.Areas.Admin.Models
{
    public class LoginViewModel
    {
        [Key]
        [MaxLength(50)]
        [Required(ErrorMessage = "Enter Email ! ")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Wrong email format")]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter Password ! ")]
        [DataType(DataType.Password)]
        [MaxLength(20, ErrorMessage = "Password only have 20 character")]
        public string Password { get; set; }
    }
}
