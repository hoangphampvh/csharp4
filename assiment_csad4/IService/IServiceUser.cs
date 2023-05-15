using assiment_csad4.Models;

namespace assiment_csad4.IService
{
    public interface IServiceUser
    {
        public bool CreateUser(User product);
        public bool UpdateUser(User product);
        public bool DeleteUser(Guid productId);
        public List<User> GetAllUser();
    }
}
