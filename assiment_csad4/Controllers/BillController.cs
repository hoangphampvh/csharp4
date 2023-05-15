using assiment_csad4.Configruration;
using assiment_csad4.IService;
using assiment_csad4.Models;
using assiment_csad4.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace assiment_csad4.Controllers
{
    public class BillController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IServiceProduct _serviceProduct;
        private readonly IServiceCartDetail _ServiceCartDetail;
        private readonly IServiceBill _serviceBill;
        private readonly IBillDetailService _billDetaliService;
        public BillController(CartDetailService serviceCartDetail, BillService billService,BillDetailService billDetailService,ProductService productService )
        {
            _context = new MyDbContext();
            _ServiceCartDetail = serviceCartDetail;
            _serviceProduct = productService;
            _serviceBill = billService;
            _billDetaliService = billDetailService;
        }
        public IActionResult ListBill()
        {
            return View(_context.Bills.Include(p => p.BillDetails).Include(p => p.UserNavigation).ToList());
        }
        [HttpGet("/Bill/Details/")]
        public IActionResult Details(Guid? Id)
        {
            var billNow = _context.Bills.FirstOrDefault(p => p.Id == Id);
            if (billNow == null)
            {
                Console.WriteLine("bill null");
                return NotFound();
            }
            var idBillNow = billNow.Id;
            var billDetail = _billDetaliService.GetAllBillDetail().Where(p => p.IdHD == idBillNow);
            if (billDetail != null)
            {
                return View(billDetail);
            }
            else
            {
                Console.WriteLine("bill detail null");
                return NotFound();

            }
        }
        [HttpPost("/Bill/Delete/{id}")]
        public IActionResult Delete(Guid? Id)
        {
            var billNow = _context.Bills.FirstOrDefault(p => p.Id == Id);
            if (billNow == null)
            {
                Console.WriteLine("bill null");
                return NotFound();
            }         
            else
            {
                _context.Remove(billNow);
                _context.SaveChanges();
                return RedirectToAction(nameof(ListBill));
            }
        }
        [HttpPost("/Bill/Confirm/")]
        public IActionResult confirm(Guid? Id )
        {
            var billNow = _context.Bills.FirstOrDefault(p => p.Id == Id);
            if (billNow == null)
            {
                Console.WriteLine("bill null");
                return NotFound();
            }
            var idBillNow = billNow.Id;
            var billDetail = _billDetaliService.GetAllCartInBill().FirstOrDefault(p => p.IdHD == idBillNow);
            if(billDetail !=null && billDetail.Product != null)
            {
                foreach (var item in _billDetaliService.GetAllBillDetail().Where(p => p.IdHD == idBillNow))
                {
                    var product = billDetail.Product;
                    product.AvailableQuantity = product.AvailableQuantity - item.Quantity;
                    if (product.AvailableQuantity == 0) product.Status = 1;
                    product.Status = 0;
                    _serviceProduct.UpdateProduct(product);                             
                }
                var cartDetail = _ServiceCartDetail.GetAllCartDetailInBill().Where(p=>p.IdProduct == billDetail.Product.Id);
                if (cartDetail != null)
                {
                    foreach (var itemCart in cartDetail)
                    {
                        Console.WriteLine("so luong sp trong don hang" + cartDetail.Count());
                            _ServiceCartDetail.UpdateCartDetailByInBill(itemCart);
                    }
                }
            }
            billNow.Status = 1;
            Console.WriteLine("Bill update status: 1 " + _serviceBill.UpdateBill(billNow));
            return RedirectToAction(nameof(ListBill));
        }

    }
}
