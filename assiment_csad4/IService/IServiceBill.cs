using assiment_csad4.Models;

namespace assiment_csad4.IService
{
    public interface IServiceBill
    {
        public bool CreateBill(Bill product);
        public bool UpdateBill(Bill product);
        public bool DeleteBill(Guid productId);
        public List<Bill> GetAllBill();
        public List<Bill> GetAllBillPay();
    }
}
