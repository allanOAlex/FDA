using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.User
{
    public record CreateUserRequest : Request
    {

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must have a minimum of 3 characters")]
        [RegularExpression(@"^[a-zA-Z0-9 _@./#&+-]{3,}$", ErrorMessage = "Username must be alphanumeric")]
        public string? UserName { get; set; }



        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "\n" +
        "Must have a minimum of 8 characters in length \n " +
        "Must contain at least one uppercase English letter \n" +
        "Must contain at least one lowercase English letter \n" +
        "Must contain at least one digit \n" +
        "Must contain at least one special character")]
        public string? Password { get; set; }


        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string? ConfirmPassword { get; set; }



        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required")]
        [MinLength(2, ErrorMessage = "First name must have a minimum of 2 characters")]
        [RegularExpression(@"^[a-zA-Z]{2,}$", ErrorMessage = "First Name must be alphabetic, with a minimum of 2 characters")]
        public string? FirstName { get; set; }



        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        [MinLength(2, ErrorMessage = "Last name must have a minimum of 2 characters")]
        [RegularExpression(@"^[a-zA-Z]{2,}$", ErrorMessage = "Last Name must be alphabetic, with a minimum of 2 characters")]
        public string? LastName { get; set; }



        [Display(Name = "Other Names")]
        [MinLength(2, ErrorMessage = "Username must have a minimum of 2 characters")]
        [RegularExpression(@"^[a-zA-Z]{2,}$", ErrorMessage = "Last Name must be alphabetic, with a minimum of 2 characters")]
        [DefaultValue("n/a")]
        public string? OtherNames { get; set; }



        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Please provide a valid email address")]
        public string? Email { get; set; }



        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact requires a minimum of 10 digits")]
        public string? PhoneNumber { get; set; }


        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }
    }
}
