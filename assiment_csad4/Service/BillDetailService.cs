using assiment_csad4.Configruration;
using assiment_csad4.IService;
using assiment_csad4.Models;
using Microsoft.EntityFrameworkCore;

namespace assiment_csad4.Service
{
    public class BillDetailService : IBillDetailService
    {
        public readonly MyDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;



        public BillDetailService(IHttpContextAccessor httpContextAccessor)
        {
            _db = new MyDbContext();
            _httpContextAccessor = httpContextAccessor;

            // truy cập vào HttpContext hiện tại 
            _httpContext = _httpContextAccessor.HttpContext;
            //có thể sử dụng biến _httpContext để truy cập vào các thông tin của HttpContext hiện tại,
            //chẳng hạn như thông tin về yêu cầu và phản hồi,
            //thông tin về người dùng đang truy cập, thông tin về session, và các thông tin khác.
        }
        public bool CreateBillDetail(BillDetail product)
        {
            try
            {
                _db.BillDetails.Add(product);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public List<BillDetail> GetAllCartInBill()
        {

                return _db.BillDetails.Include(p => p.Product).Include(p => p.Bill).ToList();
            
        }
        public bool DeleteBillDetail(Guid productId)
        {
            try
            {
                var product = _db.BillDetails.FirstOrDefault(p => p.Id == productId);
                if(product != null)
                _db.BillDetails.Remove(product);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public List<BillDetail> GetAllBillDetail()
        {
            return _db.BillDetails.Include(p => p.Product).Include(p => p.Bill).ThenInclude(p=>p.UserNavigation).ToList();
        }

        public bool UpdateBillDetail(BillDetail product)
        {
            try
            {
                var productUpdate = _db.BillDetails.FirstOrDefault(p => p.Id == product.Id);
                if (productUpdate != null)
                {
                    productUpdate.Quantity = productUpdate.Quantity+ product.Quantity;
                    _db.BillDetails.Update(productUpdate);
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
