using assiment_csad4.Models;

namespace assiment_csad4.IService
{
    public interface ICartService
    {
        public bool CreateCart(Cart product);
        public bool UpdateCart(Cart product);
        public bool DeleteCart(Guid productId);
        public List<Cart> GetAllCart();
    }
}
