using assiment_csad4.Configruration;
using assiment_csad4.IService;
using assiment_csad4.Models;

namespace assiment_csad4.Service
{
    public class RoleService : IServiceRole
    {
        public readonly MyDbContext _db;
        public RoleService()
        {
            _db = new MyDbContext();
        }
        public bool CreateRole(Role product)
        {
            try
            {
                _db.Roles.Add(product);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool DeleteRole(Guid productId)
        {
            try
            {
                var product = _db.Roles.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                _db.Roles.Remove(product);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public List<Role> GetAllRole()
        {
            return _db.Roles.ToList();
        }

        public bool UpdateRole(Role product)
        {
            try
            {
                var productUpdate = _db.Roles.FirstOrDefault(p => p.Id == product.Id);
                if (productUpdate != null)
                {
                    productUpdate.Name = product.Name;
                    productUpdate.Description = product.Description;
                    productUpdate.Status = product.Status;
                    _db.Roles.Update(productUpdate);
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
