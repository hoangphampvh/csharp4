using assiment_csad4.Configruration;
using assiment_csad4.IService;
using assiment_csad4.Models;
using assiment_csad4.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace assiment_csad4.Service
{
    public class CartDetailService : IServiceCartDetail
    {
        public readonly MyDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        public CartDetailService(IHttpContextAccessor httpContextAccessor)
        {
            _db = new MyDbContext();
            _httpContextAccessor = httpContextAccessor;

            // truy cập vào HttpContext hiện tại 
            _httpContext = _httpContextAccessor.HttpContext;
            //có thể sử dụng biến _httpContext để truy cập vào các thông tin của HttpContext hiện tại,
            //chẳng hạn như thông tin về yêu cầu và phản hồi,
            //thông tin về người dùng đang truy cập, thông tin về session, và các thông tin khác.
        }

        public bool CreateCartDetail(Models.CartDetail product)
        {
            try
            {
                if (_httpContext.User.Identity != null)
                {
                    var name = _httpContext.User.Identity.Name;

                    if (name != null && _db.Users.FirstOrDefault(p => p.Name == name) != null)
                    {
                        var User = _db.Users.FirstOrDefault(p => p.Name == name);
                        if (User != null)
                        {
                            product.IdUser = User.Id;
                            product.Id = Guid.NewGuid();
                            Console.WriteLine("ID User: " + User.Id.ToString());
                            Console.WriteLine("ID Product: " + product.IdProduct.ToString());
                            _db.CartDetails.Add(product);
                            _db.SaveChanges();
                        }
                        else Console.WriteLine("user null");

                    }
                    else
                    {
                        Console.WriteLine("dsdsd");
                    }
                }
                return true;
            }
            catch (Exception)
            {

                return false;
                throw;
            }
        }

        public bool DeleteCartDetail(Guid productId)
        {
            try
            {
                var product = _db.CartDetails.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                    _db.CartDetails.Remove(product);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public List<Models.CartDetail> GetAllCartDetail()
        {
            if (_httpContext.User.Identity != null)
            {
                var name = _httpContext.User.Identity.Name;

                if (name != null && _db.Users.FirstOrDefault(p => p.Name == (name)) != null)
                {
                    var User1 = _db.Users.FirstOrDefault(p => p.Name == name);
                    if (User1 != null)
                    {
                        return _db.CartDetails.Include(p => p.ProductNavigation)
                       .Where(p => p.IdUser == User1.Id).ToList();
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
        public List<Models.CartDetail> GetAllCartDetailInBill()
        {
                        return _db.CartDetails.Include(p => p.ProductNavigation).ToList();
        }
        public bool UpdateCartDetailStatus(CartDetail product)
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
                            var productUpdate = _db.CartDetails.FirstOrDefault(p => p.IdProduct == product.IdProduct);
                            if (productUpdate != null)
                            {
                                Console.WriteLine("ID User: " + User.Id.ToString());
                                Console.WriteLine("ID Product: " + product.IdProduct.ToString());
                                _db.CartDetails.Update(productUpdate);
                                _db.SaveChanges();
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("productUpdate null");
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
        public bool UpdateCartDetail(Models.CartDetail product)
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
                            var productUpdate = _db.CartDetails.FirstOrDefault(p => p.IdProduct == product.IdProduct);
                            if (productUpdate != null)
                            {
                                Console.WriteLine("ID User: " + User.Id.ToString());
                                Console.WriteLine("ID Product: " + product.IdProduct.ToString());

                                productUpdate.Quantilty = product.Quantilty;
                                productUpdate.IdProduct = product.IdProduct;
                                productUpdate.Status = product.Status;
                                productUpdate.IdUser = User.Id;
                                _db.CartDetails.Update(productUpdate);
                                _db.SaveChanges();
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("productUpdate null");
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
        public bool UpdateCartDetailByInBill(Models.CartDetail product)
        {
            try
            {

                                 product.Status = 1;                
                                _db.CartDetails.Update(product);
                                _db.SaveChanges();
                                return true;                                                  
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public const string Key = "Session_Key1";
        public List<CartSession> GetItemFromSession()
        {
            var session = _httpContext.Session;

            var data = session.GetString("Session_Key1");
            if (data != null) return JsonConvert.DeserializeObject<List<CartSession>>(data);
            return new List<CartSession>();
        }
        public void RemoveData()
        {
            _httpContext.Session.Remove("Session_Key1");
        }
        public void SetDataToSession(List<CartSession> products)
        {
            var session = _httpContext.Session;
            var DataJson = JsonConvert.SerializeObject(products);
            session.SetString("Session_Key1", DataJson);
        }
    }
}
