using assiment_csad4.Configruration;
using assiment_csad4.IService;
using assiment_csad4.Models;

namespace assiment_csad4.Service
{
    public class CartService : ICartService
    {
        public readonly MyDbContext _db;
        public CartService()
        {
            _db = new MyDbContext();
        }
        public bool CreateCart(Cart product)
        {
            try
            {
                _db.Carts.Add(product);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool DeleteCart(Guid productId)
        {
            try
            {
                var product = _db.Carts.FirstOrDefault(p => p.IdUser == productId);
                _db.Carts.Remove(product);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public List<Cart> GetAllCart()
        {
            return _db.Carts.ToList();
        }

        public bool UpdateCart(Cart product)
        {
            try
            {
                var productUpdate = _db.Carts.FirstOrDefault(p => p.IdUser == product.IdUser);
                productUpdate.Description = product.Description;
                _db.Carts.Update(productUpdate);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
