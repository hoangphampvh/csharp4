using assiment_csad4.Models;

namespace assiment_csad4.IService
{
    public interface IServiceProduct
    {
        public bool CreateProduct(Product product);
        public bool UpdateProduct(Product product);
        public bool UpdateProductBuy(Product product);

        public bool DeleteProduct(Guid productId);
        public List<Product> GetAllProduct();
        public bool UpdateProductFullPropety(Product product);
        public void SetDataToSession(List<Product> products);
        public List<Product> GetItemFromSession();
        public List<Product> GetItemNewFromSession();
        public void SetDataNewToSession(List<Product> products);
        public bool UpdateProductImage(Product product);
    }
}
