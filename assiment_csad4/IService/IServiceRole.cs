using assiment_csad4.Models;

namespace assiment_csad4.IService
{
    public interface IServiceRole
    {
        public bool CreateRole(Role product);
        public bool UpdateRole(Role product);
        public bool DeleteRole(Guid productId);
        public List<Role> GetAllRole();
    }
}
