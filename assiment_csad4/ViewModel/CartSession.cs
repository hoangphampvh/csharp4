using assiment_csad4.Models;

namespace assiment_csad4.ViewModel
{
    public class CartSession : Product
    {
       public int count { get; set; }
       public Product? Product { get; set; }
    }
}
