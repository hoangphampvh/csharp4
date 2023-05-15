using assiment_csad4.Configruration;
using assiment_csad4.IService;
using assiment_csad4.Models;
using assiment_csad4.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace assiment_csad4.Service
{
    public class ProductService : IServiceProduct
    {
        public readonly MyDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly string Key = "Session_Key";
        private readonly string Key2 = "Session_Key2";


        public ProductService(IHttpContextAccessor httpContextAccessor)
        {
            _db = new MyDbContext();
            _httpContextAccessor = httpContextAccessor;

            // truy cập vào HttpContext hiện tại 
            _httpContext = _httpContextAccessor.HttpContext;
            //có thể sử dụng biến _httpContext để truy cập vào các thông tin của HttpContext hiện tại,
            //chẳng hạn như thông tin về yêu cầu và phản hồi,
            //thông tin về người dùng đang truy cập, thông tin về session, và các thông tin khác.
        }
        public bool CreateProduct(Product product)
        {
            try
            {
                product.CreateDate = DateTime.Now;
                _db.Products.Add(product);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
           
        }

        public bool DeleteProduct(Guid productId)
        {
            try
            {
                var product = _db.Products.FirstOrDefault(p => p.Id == productId);
                if(product !=null)
                _db.Products.Remove(product);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public List<Product> GetAllProduct()
        {
           return _db.Products.ToList();
        }
        public List<Product> GetItemFromSession()
        {
            var session = _httpContext.Session;
            var data = session.GetString(Key);
            if (data != null) return JsonConvert.DeserializeObject<List<Product>>(data);
            return new List<Product>();
        }
        public List<Product> GetItemNewFromSession()
        {
            var session = _httpContext.Session;
            var data = session.GetString(Key2);
            if (data != null) return JsonConvert.DeserializeObject<List<Product>>(data);
            return new List<Product>();
        }
        public void SetDataNewToSession(List<Product> products)
        {
            var session = _httpContext.Session;
            var DataJson = JsonConvert.SerializeObject(products);
            session.SetString(Key2, DataJson);
        }
        public void SetDataToSession(List<Product> products)
        {
            var session = _httpContext.Session;
            var DataJson = JsonConvert.SerializeObject(products);
            session.SetString(Key, DataJson);
        }
        public bool UpdateProductFullPropety(Product product)
        {
            try
            {
                var productUpdate = _db.Products.FirstOrDefault(p => p.Id == product.Id);
                if (productUpdate != null)
                {
                    productUpdate.Name = product.Name;
                    productUpdate.Price = product.Price;
                    productUpdate.Supplier = product.Supplier;
                    productUpdate.Description = product.Description;
                    productUpdate.Status = product.Status;
                    productUpdate.AvailableQuantity = product.AvailableQuantity;
                    productUpdate.UrlImage = product.UrlImage;
                    _db.Products.Update(productUpdate);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public bool UpdateProduct(Product product)
        {
            try
            {
                var productUpdate = _db.Products.FirstOrDefault(p => p.Id == product.Id);
                if (productUpdate != null)
                {
                    productUpdate.Status = product.Status;
                    productUpdate.AvailableQuantity = product.AvailableQuantity;
                    _db.Products.Update(productUpdate);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public bool UpdateProductImage(Product product)
        {
            try
            {
                var productUpdate = _db.Products.FirstOrDefault(p => p.Id == product.Id);
                if (productUpdate != null)
                {
                    productUpdate.UrlImage = product.UrlImage;
                    _db.Products.Update(productUpdate);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public bool UpdateProductBuy(Product product)
        {
            try
            {
                var productUpdate = _db.Products.FirstOrDefault(p => p.Id == product.Id);
                if(productUpdate != null)
                {                                 
                    productUpdate.AvailableQuantity = product.AvailableQuantity;
                    _db.Products.Update(productUpdate);
                    _db.SaveChanges();
                    return true;
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
