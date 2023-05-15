using assiment_csad4.Configruration;
using assiment_csad4.IService;
using assiment_csad4.Models;

namespace assiment_csad4.Service
{
    public class UserService : IServiceUser
    {
        public readonly MyDbContext _db;
        public UserService()
        {
            _db = new MyDbContext();
        }
        public bool CreateUser(User product)
        {
            try
            {
                _db.Users.Add(product);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public bool DeleteUser(Guid productId)
        {
            try
            {
                var product = _db.Users.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    _db.Users.Remove(product);
                    _db.SaveChanges();
                }
              
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public List<User> GetAllUser()
        {
            return _db.Users.ToList();
        }

        public bool UpdateUser(User product)
        {
            try
            {
                var productUpdate = _db.Users.FirstOrDefault(p => p.Id == product.Id);
                if(productUpdate != null)
                {
                    productUpdate.Name = product.Name;
                    productUpdate.Pass = product.Pass;
                    productUpdate.Status = product.Status;
                    _db.Users.Update(productUpdate);
                    _db.SaveChanges();
                }
               
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
