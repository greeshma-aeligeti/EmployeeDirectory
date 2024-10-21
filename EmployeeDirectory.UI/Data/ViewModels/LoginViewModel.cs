using System.ComponentModel.DataAnnotations;

namespace EmployeeDirectory.UI.Data.ViewModels
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="Please provide User Name")]
        public string? UserName {  get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide Password")]

        public string? Password { get; set; }
    }
}
