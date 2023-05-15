using assiment_csad4.Models;
using System.ComponentModel.DataAnnotations;

namespace assiment_csad4.ViewModel
{
    public class SignUp : User
    {
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
