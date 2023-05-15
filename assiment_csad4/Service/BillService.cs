using assiment_csad4.Configruration;
using assiment_csad4.IService;
using assiment_csad4.Models;
using Microsoft.EntityFrameworkCore;

namespace assiment_csad4.Service
{
    public class BillService : IServiceBill
    {
        public readonly MyDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;

        public BillService(IHttpContextAccessor httpContextAccessor)
        {
            _db = new MyDbContext();
            _httpContextAccessor = httpContextAccessor;

            // truy cập vào HttpContext hiện tại 
            _httpContext = _httpContextAccessor.HttpContext;
            //có thể sử dụng biến _httpContext để truy cập vào các thông tin của HttpContext hiện tại,
            //chẳng hạn như thông tin về yêu cầu và phản hồi,
            //thông tin về người dùng đang truy cập, thông tin về session, và các thông tin khác.
        }
        public bool CreateBill(Bill product)
        {
            try
            {
                _db.Bills.Add(product);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool DeleteBill(Guid productId)
        {
            try
            {
                var product = _db.Bills.FirstOrDefault(p => p.Id == productId);
                _db.Bills.Remove(product);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
       
        public List<Bill> GetAllBill()
        {
            if (_httpContext.User.Identity != null)
            {
                var name = _httpContext.User.Identity.Name;

                if (name != null && _db.Users.FirstOrDefault(p => p.Name == (name)) != null)
                {
                    var User = _db.Users.FirstOrDefault(p => p.Name == (name));
                    if (User != null)
                    {
                        return _db.Bills.Include(p => p.BillDetails).Where(p => p.UserId == User.Id).ToList();
                    }
                    else Console.WriteLine("null id");
                }
                else Console.WriteLine("null name");
            }
            else
            {
                Console.WriteLine("chua dang ky");
            }
            return null;
        }
        public List<Bill> GetAllBillPay()
        {    
                       return _db.Bills.Include(p => p.BillDetails).ToList();
        }
        public bool UpdateBill(Bill product)
        {
            try
            {
                if (_httpContext.User.Identity != null)
                {
                    var name = _httpContext.User.Identity.Name;

                    if (name != null && _db.Users.FirstOrDefault(p => p.Name == (name)) != null)
                    {
                        var User = _db.Users.FirstOrDefault(p => p.Name == (name));
                        if (User != null)
                        {
                            var BillUpdate = _db.Bills.Include(p=>p.BillDetails).FirstOrDefault(p => p.Id == product.Id);
                            if (BillUpdate != null)
                            {
                                Console.WriteLine("ID User: " + User.Id.ToString());
                                Console.WriteLine("ID Bill: " + product.Id.ToString());

                                BillUpdate.Status = product.Status;
                                BillUpdate.UserId = product.UserId;
                                BillUpdate.Status = product.Status;
                                BillUpdate.CreateDate = DateTime.Now;
                                _db.Bills.Update(BillUpdate);
                                _db.SaveChanges();
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("BillUpdate null");
                            }
                        }

                    }

                }

                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
