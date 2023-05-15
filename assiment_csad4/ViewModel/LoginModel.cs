using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace assiment_csad4.ViewModel
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public bool RememberLogin { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
