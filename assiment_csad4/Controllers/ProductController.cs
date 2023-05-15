using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using assiment_csad4.Configruration;
using assiment_csad4.Models;
using assiment_csad4.IService;
using assiment_csad4.Service;
using Microsoft.AspNetCore.Authorization;
using CartDetail = assiment_csad4.Models.CartDetail;
using Microsoft.CodeAnalysis;
using assiment_csad4.ViewModel;

namespace assiment_csad4.Controllers
{
    public class ProductController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IServiceProduct _serviceProduct;
        private readonly IServiceCartDetail _ServiceCartDetail;

        public const int ItemsInPage = 10;

        public ProductController(IServiceCartDetail serviceCartDetail, ProductService productService)
        {
            _context = new MyDbContext();
            _serviceProduct = productService;
            _ServiceCartDetail = serviceCartDetail;
        }
        [HttpGet("Product/search")]
        public async Task<IActionResult> search(int pagesize, [FromQuery(Name = "p")] int currenPage, string name)
        {

            if (!string.IsNullOrEmpty(name))
            {
                var ListPostByName = _context.Products.Where(p => p.Name.ToLower().Contains(name.ToLower()));
                int totalPosts = await ListPostByName.CountAsync();
                if (pagesize <= 1) pagesize = 10;
                int countPage = (int)Math.Ceiling((double)totalPosts / pagesize);

                if (currenPage > countPage) currenPage = countPage;
                if (currenPage <=1) currenPage = 1;

                var pagingModel = new PagingModel()
                {
                    countpages = countPage,
                    currentpage = currenPage,
                    generateUrl = (pageNumber) => Url.Action("Index",
                    new
                    {
                        p = pageNumber,
                        pagesize = pagesize
                    })

                };
                ViewBag.pagingModel = pagingModel;

                var postsInPage = await ListPostByName
                    .Skip((currenPage - 1) * pagesize)
                    .Take(pagesize).ToListAsync();
                return View("Index", postsInPage);
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Product
        public async Task<IActionResult> Index([FromQuery(Name ="page")]int nowPage, int PageSize)
        {
            var cartDetail = new CartDetail();
            var cartInSession = _ServiceCartDetail.GetItemFromSession();
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                foreach (var item in cartInSession)
                {
                    if (_ServiceCartDetail.GetAllCartDetail().Any(p => p.IdProduct == item.Product.Id))
                    { 
                        cartDetail.Quantilty = _ServiceCartDetail.GetAllCartDetail().FirstOrDefault(p => p.IdProduct == item.Product.Id).Quantilty + item.count;
                        cartDetail.Status = 0;
                        cartDetail.IdProduct = item.Product.Id;
                        Console.WriteLine("session " + item.count);
                        if (_ServiceCartDetail.UpdateCartDetail(cartDetail))
                        {
                            Console.WriteLine("update from session");
                            _ServiceCartDetail.RemoveData();
                        }
                        else
                        {
                            Console.WriteLine("update from session fales");
                        }

                    }
                    else
                    {
                        cartDetail.Quantilty = item.count;
                        cartDetail.IdProduct = item.Product.Id;
                        cartDetail.Status = 0;
                        if (_ServiceCartDetail.CreateCartDetail(cartDetail))
                        {
                            Console.WriteLine("add from session");
                            _ServiceCartDetail.RemoveData();
                        }
                        else
                        {
                            Console.WriteLine("add from session fales");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("sai");
            }
            
            int SumPage = await _context.Products.CountAsync();
            if (PageSize <= 1) PageSize = 10;

            int countPage = (int)Math.Ceiling((double)SumPage / PageSize);
            if (nowPage > countPage) nowPage = countPage;
            if (nowPage <= 1) nowPage = 1;

            var pagingModel = new PagingModel() 
            {
                countpages = countPage,
                currentpage = nowPage, 
                generateUrl = (pageNumber) => Url.Action("Index",
                new 
                { 
                    page = pageNumber,
                    PageSize = PageSize 
                })
            };
            ViewBag.pagingModel = pagingModel;
            var productInPage =await _context.Products.Skip((nowPage - 1) * PageSize).Take(PageSize).ToListAsync();


            return View(productInPage);
        }

        // GET: Product/Details/5        

        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost("/Product/Details/{id:Guid}")]
        public async Task<IActionResult> Details([FromRoute] Guid id, [Bind("Quantilty,Status")] CartDetail cartDetailUpdate, int count)
        {
            if (!User.Identity.IsAuthenticated) 
            {
                var productToSession = _serviceProduct.GetAllProduct().FirstOrDefault(p => p.Id == id);
                if (productToSession == null) return BadRequest();
                List<CartSession> products = _ServiceCartDetail.GetItemFromSession();
                var item = products.FirstOrDefault(p => p.Product.Id == id);
                if (item != null)
                {
                    item.count = item.count + count;
                    ViewBag.mess = " <p class=\"alert alert-danger\">Them vao gio hang thanh cong</p>";
                }
                else
                {
                    products.Add(new CartSession() { count = count, Product = productToSession });
                    ViewBag.mess = " <p class=\"alert alert-danger\">Them vao gio hang thanh cong</p>";
                }
                _ServiceCartDetail.SetDataToSession(products);
                return View(productToSession);
            }
            else
            {
                _ServiceCartDetail.RemoveData();
                Console.WriteLine("so luong:" + count.ToString());
                var product = await _context.Products
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                var cartNow = _ServiceCartDetail.GetAllCartDetail().FirstOrDefault(p => p.IdProduct == product.Id);
               
                 if (cartNow != null)
                {
                    cartDetailUpdate.Quantilty = cartNow.Quantilty + count;
                    cartDetailUpdate.IdProduct = id;
                    cartDetailUpdate.Status = 0;
                    Console.WriteLine("update" + _ServiceCartDetail.UpdateCartDetail(cartDetailUpdate));
                    if (_ServiceCartDetail.UpdateCartDetail(cartDetailUpdate))
                    {
                        ViewBag.mess = " <p class=\"alert alert-danger\">Them vao gio hang thanh cong</p>";
                    }
                    return View(product);
                }
                else
                {
                    if(count > product.AvailableQuantity)
                    {
                        count = product.AvailableQuantity;
                    }
                    cartDetailUpdate.Quantilty = count;             
                    Console.WriteLine(count.ToString());
                    cartDetailUpdate.IdProduct = id;
                    cartDetailUpdate.Status = 0;
                    if (_ServiceCartDetail.CreateCartDetail(cartDetailUpdate))
                    {
                        ViewBag.mess = " <p class=\"alert alert-danger\">Them vao gio hang thanh cong</p>";
                    }
                }        
                return View(product);
            }
        }
        // GET: Product/Create
        public IActionResult Create()
        {
            var data = new object[]
          {
                new
                {
                    value =1,
                    status = "Con Hang"
                },
                new
                {
                     value =0,
                    status = "het Hang"
                }
          };
            ViewData["listStatus"] = new SelectList(data, "value", "status");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Price,AvailableQuantity,Status,Supplier,Description,UrlImage")] Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0) // Không null không rỗng
                {
                    // Thực hiện trỏ tới thư mục root để lát thực hiện việc copy
                    var path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "images", imageFile.FileName); // Bước 2
                                                                  // Kết quả: aaa/wwwroot/images/xxx.jpg, Path.Combine = tổng hợp đường dẫn
                    Console.WriteLine(path);
                    var stream = new FileStream(path, FileMode.Create);
                    // Vì chúng ta thực hiện việc copy => Tạo mới => Create
                    imageFile.CopyTo(stream); // Copy ảnh chọn ở form vào wwwroot/images
                                              // Gán lại giá trị cho thuộc tính Description => Bước 3
                    product.UrlImage = imageFile.FileName; // Bước 4
                }
                    _serviceProduct.CreateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            var data = new object[]
         {
                new
                {
                    value =1,
                    status = "Con Hang"
                },
                new
                {
                     value =0,
                    status = "het Hang"
                }
         };
            ViewData["listStatus"] = new SelectList(data, "value", "status");
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var data = new object[]
         {
                new
                {
                    value =1,
                    status = "Con Hang"
                },
                new
                {
                     value =0,
                    status = "het Hang"
                }
         };
            ViewData["listStatus"] = new SelectList(data, "value", "status");
            return View(product);
        }
        [HttpGet("/ListProductOld")]
        public IActionResult ShowOldData()
        {
            ViewData["product"] = _serviceProduct.GetItemNewFromSession();
            return View(_serviceProduct.GetItemFromSession());
        }
        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Name,Price,AvailableQuantity,Status,Supplier,Description,UrlImage")] Product product, IFormFile? imageFilee)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            var productOld = _serviceProduct.GetAllProduct().FirstOrDefault(p => p.Id == id);
            var listProductOldInSession = _serviceProduct.GetItemFromSession();
            var productInSession = listProductOldInSession.FirstOrDefault(p => p.Id == id);
            if(productInSession != null)
            {
                listProductOldInSession.Remove(productInSession);
            }
            if (productOld != null)
            {
                listProductOldInSession.Add(productOld);
                _serviceProduct.SetDataToSession(listProductOldInSession);
            }
           
            if (ModelState.IsValid)
            {
                try
                {                  
                    var ListNewProduct = _serviceProduct.GetItemNewFromSession();
                    var productNew = ListNewProduct.FirstOrDefault(p => p.Id == id);
                    if (productNew != null)
                    {
                        ListNewProduct.Remove(productNew);
                    }
                    if (imageFilee != null && imageFilee.Length > 0) // Không null không rỗng
                    {
                        // Thực hiện trỏ tới thư mục root để lát thực hiện việc copy
                        var path = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot", "images", imageFilee.FileName); // Bước 2
                                                                      // Kết quả: aaa/wwwroot/images/xxx.jpg, Path.Combine = tổng hợp đường dẫn
                        Console.WriteLine("path: " +path);
                        var stream = new FileStream(path, FileMode.Create);
                        // Vì chúng ta thực hiện việc copy => Tạo mới => Create
                        imageFilee.CopyTo(stream); // Copy ảnh chọn ở form vào wwwroot/images
                                                  // Gán lại giá trị cho thuộc tính Description => Bước 3
                        product.UrlImage = imageFilee.FileName; // Bước 4
                    }
                    else
                    {
                        product.UrlImage = productOld.UrlImage;
                    }
                    _serviceProduct.SetDataToSession(listProductOldInSession);
                    ListNewProduct.Add(product);
                    _serviceProduct.SetDataNewToSession(ListNewProduct);
                    ViewData["product"] = ListNewProduct;

                    _serviceProduct.UpdateProductFullPropety(product);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var data = new object[]
         {
                new
                {
                    value =1,
                    status = "Con Hang"
                },
                new
                {
                     value =0,
                    status = "het Hang"
                }
         };
            ViewData["listStatus"] = new SelectList(data, "value", "status");
            return View(product);
        }
        [HttpPost("RollBack/{id?}")]
        public  IActionResult RollBack(Guid?id )
        {
            var listOld = _serviceProduct.GetItemFromSession();
            var listNew = _serviceProduct.GetItemNewFromSession();
            var productNew = listNew.FirstOrDefault(p => p.Id == id);
            if (productNew != null)
            {
               

                var ProductOld = listOld.FirstOrDefault(x => x.Id == id);
                if (ProductOld!= null)
                {
                    Console.WriteLine(listNew.Remove(productNew));

                    productNew = ProductOld;
                    if (_serviceProduct.UpdateProductFullPropety(productNew))
                    {
                        Console.WriteLine(listOld.Remove(ProductOld));
                        _serviceProduct.SetDataToSession(listOld);
                        _serviceProduct.SetDataNewToSession(listNew);

                        return RedirectToAction(nameof(Index));
                    }
                    return BadRequest();
                }
                return BadRequest();
            }
           
            else
            {
                return BadRequest();
            }

        }
        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
       
       
    }
}
