using assiment_csad4.Models;

namespace assiment_csad4.IService
{
    public interface IBillDetailService
    {
        public bool CreateBillDetail(BillDetail product);
        public bool UpdateBillDetail(BillDetail product);
        public bool DeleteBillDetail(Guid productId);
        public List<BillDetail> GetAllCartInBill();
        public List<BillDetail> GetAllBillDetail();
    }
}
   

