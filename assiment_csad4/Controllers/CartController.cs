using assiment_csad4.Configruration;
using assiment_csad4.IService;
using assiment_csad4.Models;
using assiment_csad4.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using System;
using assiment_csad4.ViewModel;
using System.Linq;

namespace assiment_csad4.Controllers
{
    public class CartController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IServiceProduct _serviceProduct;
        private readonly IServiceCartDetail _ServiceCartDetail;
        private readonly IServiceBill _serviceBill;
        private readonly IBillDetailService _billDetaliService;
        public CartController(CartDetailService serviceCartDetail, BillService billService, BillDetailService BillDetailService,ProductService productService)
        {
            _context = new MyDbContext();
            _ServiceCartDetail = serviceCartDetail;
            _serviceProduct = productService;
            _serviceBill = billService;
            _billDetaliService = BillDetailService;
        }
        [Authorize]
        [HttpGet("/ListCart")]
        public IActionResult ListCart()
        {
            var cartDetails = _ServiceCartDetail.GetAllCartDetail().Where(p => p.Status == 0);
           
            return View(cartDetails);
        }

        [HttpPost("/DeleteCart/{id?}")]
        public IActionResult DeleteCart(Guid id)
        {
            List<CartSession> products = _ServiceCartDetail.GetItemFromSession();
            var item = products.FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                products.Remove(item);
                _ServiceCartDetail.SetDataToSession(products);
            }
            return RedirectToAction(nameof(ShowCart));
        }
        /// Cập nhật
        [HttpPost("/UpdateCart/{id}")]
        public IActionResult UpdateCart(Guid id, [FromForm] int count)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            List<CartSession> cart = _ServiceCartDetail.GetItemFromSession();
         
            foreach (var item in cart.Where(p=>p.Product.Id == id))
            {
                item.count = count;
            }
            _ServiceCartDetail.SetDataToSession(cart);
            return RedirectToAction(nameof(ShowCart));
        }
        [HttpPost("/addCart/{id?}")]
        public IActionResult addToCartWithSession(Guid Id)
        {
            var product = _serviceProduct.GetAllProduct().FirstOrDefault(p => p.Id == Id);
            if (product == null) return BadRequest();
            List<CartSession> products = _ServiceCartDetail.GetItemFromSession();
            var item = products.FirstOrDefault(p => p.Product.Id == Id);
            if (item != null)
            {
                item.count++;
            }
            else
            {
                products.Add(new CartSession() { count = 1, Product = product });
            }
            _ServiceCartDetail.SetDataToSession(products);
            return RedirectToAction(nameof(ShowCart));
        }
        [HttpGet("/ListCarts")]
        public IActionResult ShowCart()
        {
            return View(_ServiceCartDetail.GetItemFromSession());
        }

        [Authorize]
        [HttpPost("/addToCart/{id?}")]
        public IActionResult addToCart(Guid id, [Bind("Quantilty,Status")] CartDetail cartDetail)
        {
            var product = _serviceProduct.GetAllProduct().Where(p => p.Id == id);
            var CartNow = _ServiceCartDetail.GetAllCartDetail().FirstOrDefault(p => p.IdProduct == id && p.Status == 0);                         
            if (CartNow != null)
            {
                cartDetail.Quantilty = CartNow.Quantilty + 1;
                Console.WriteLine(cartDetail.Quantilty + "=" + CartNow.Quantilty + "+" + 1);
                cartDetail.IdProduct = id;
                cartDetail.Status = 0;

                if (_ServiceCartDetail.UpdateCartDetail(cartDetail))
                {
                    return RedirectToAction(nameof(ListCart));
                }
                else return BadRequest();
            }
            else
            {
                cartDetail.Quantilty = 1;
                cartDetail.IdProduct = id;
                cartDetail.Status = 0;
                if (_ServiceCartDetail.CreateCartDetail(cartDetail))
                {
                    return RedirectToAction(nameof(ListCart));
                }
                else return BadRequest();

            }
        }
        [HttpPost("/DeleteConfirmed/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            var cart = await _context.CartDetails.FindAsync(id);
            if (cart != null)
            {
                _context.CartDetails.Remove(cart);
            }
            else
                Console.WriteLine("null");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListCart));
        }
        [Authorize]
        [HttpPost("/Cart/ThanhToan")]
        [ValidateAntiForgeryToken]
        public IActionResult ThanhToan([Bind("Id,Quantilty,IdUser,IdProduct,Status")] CartDetail cartDetail)
        {

            Console.WriteLine(cartDetail.IdProduct);
            Console.WriteLine(cartDetail.IdUser);

            var cartNow = _ServiceCartDetail.GetAllCartDetail().Where(p => p.Status ==0);
            Bill bill = new Bill();
            bill.Status = 2;
            bill.CreateDate = DateTime.Now;
            bill.UserId = cartDetail.IdUser;
            bill.MaHd = "MaHd" + Guid.NewGuid();
            if (_serviceBill.CreateBill(bill))
            {
                Console.WriteLine("bill add true");
                foreach (var item in cartNow)
                {
                    BillDetail billDetail = new BillDetail();
                    billDetail.IdHD = bill.Id;
                    billDetail.IdSP = item.IdProduct;
                    billDetail.Quantity = item.Quantilty;

                    if (item.ProductNavigation != null)
                    {
                        (billDetail.Price) = Convert.ToInt32(item.ProductNavigation.Price);
                    }
                    if (_billDetaliService.CreateBillDetail(billDetail))
                    {
                        Console.WriteLine("billDetail Add True");
                    }
                }

            }
            foreach (var item in cartNow)
            {
                item.Status = 2;
                _ServiceCartDetail.UpdateCartDetailStatus(item);
            }          

            return RedirectToAction(nameof(ListCart));
        }
        [HttpPost("/Update/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromRoute] Guid id, [Bind("Id,Quantilty,IdUser,IdProduct,Status")] CartDetail cartDetail, IFormCollection form)
        {

            if (id != cartDetail.Id)
            {
                return NotFound();
            }
            Console.WriteLine(cartDetail.Quantilty);
            if (ModelState.IsValid)
            {
                Console.WriteLine(cartDetail.IdUser);
                Console.WriteLine(cartDetail.IdProduct);
                cartDetail.Quantilty = int.Parse(form["Quantilty"]);
                cartDetail.Status = 0;
                Console.WriteLine(cartDetail.Quantilty);
                Console.WriteLine(_ServiceCartDetail.UpdateCartDetail(cartDetail));
                if (_ServiceCartDetail.UpdateCartDetail(cartDetail))
                {
                    return RedirectToAction(nameof(ListCart));
                }

            }

            return RedirectToAction(nameof(ListCart));
        }
        [HttpGet("/Cart/Bought")]
        public IActionResult searchCartByStatus1()
        {
            return View( _ServiceCartDetail.GetAllCartDetail().Where(p => p.Status == 1));
        }
        [HttpGet("/Cart/Buy")]
        public IActionResult searchCartByStatus0()
        {
            return View("ListCart", _ServiceCartDetail.GetAllCartDetail().Where(p => p.Status == 0));
        }
        [HttpGet("/Cart/WaitConfirm")]
        public IActionResult searchCartByStatus2()
        {
            List<Bill> bill = _serviceBill.GetAllBill().Where(p => p.Status == 2).ToList();
            return View(bill);
        }
    }
}
