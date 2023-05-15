using assiment_csad4.Models;
using assiment_csad4.ViewModel;

namespace assiment_csad4.IService
{
    public interface IServiceCartDetail
    {
        public bool CreateCartDetail(CartDetail product);
        public bool UpdateCartDetail(CartDetail product);
        public bool UpdateCartDetailStatus(CartDetail product);
        public bool UpdateCartDetailByInBill(CartDetail product);
        public bool DeleteCartDetail(Guid productId);
        public List<CartDetail> GetAllCartDetail();
        public void RemoveData();
        public void SetDataToSession(List<CartSession> products);
        public List<CartSession> GetItemFromSession();
        public List<CartDetail> GetAllCartDetailInBill();

    }
}
