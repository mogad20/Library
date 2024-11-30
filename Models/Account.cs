using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [ValidateNever]
        public string Address { get; set; }
        

		[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid email.")]
        public string Email { get; set; }

        [NotMapped]
        [Compare("Email", ErrorMessage = "Email and Confirm Email do not match")]
        public string ReEmail { get; set; }

        [MinLength(4, ErrorMessage = "Password mustn't be less than 4 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string RePassword { get; set; }

        [RegularExpression(
        @"^(?:\+20|0020|0)?1[0-2]\d{8}$" , ErrorMessage = "Phone Number must be 11 number")]
        public string PhoneNumber { get; set; }
        public bool IsAgree { get; set; }

        public ICollection<AccountBook> AccountBooks { get; set; } = new List<AccountBook>();
    }
}
