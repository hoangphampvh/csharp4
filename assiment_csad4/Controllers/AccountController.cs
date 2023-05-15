using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using assiment_csad4.Configruration;
using assiment_csad4.ViewModel;
using assiment_csad4.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using assiment_csad4.IService;
using assiment_csad4.Service;

namespace assiment_csad4.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IServiceCartDetail _serviceCartDetail;
        public AccountController(CartDetailService cartDetailService)
        {
            _serviceCartDetail = cartDetailService;
            _context = new MyDbContext();
        }
        [HttpGet("/Account/Login")]
        public IActionResult Login(string ReturnUrl = "/")
        {
            LoginModel objLoginModel = new LoginModel();
            objLoginModel.ReturnUrl = ReturnUrl;
            return View(objLoginModel);
        }
        [HttpPost("/Account/Login")]
        public async Task<IActionResult> Login(LoginModel objLoginModel)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Where(x => x.Name == objLoginModel.UserName && x.Pass == objLoginModel.Password).FirstOrDefault();
                if( objLoginModel!=null && objLoginModel.Password.Length <= 2)
                {
                    ViewBag.mess = "so ky tu khong hop le";
                }
                if (user == null)
                {
                    //Add logic here to display some message to user
                    ViewBag.Message = "Invalid Credential";
                    return View(objLoginModel);
                }
                else if(user !=null && user.Status ==0)
                {
                    var role = _context.Roles.Where(x => x.Id == user.IdRole).FirstOrDefault();
                    if(role != null && role.Status ==0)
                    {
                        Console.WriteLine(objLoginModel.ReturnUrl);
                        //A claim is a statement about a subject by an issuer and
                        //represent attributes of the subject that are useful in the context of authentication and authorization operations.
                        // cái thằng này chứa các thông tin, quyền của người dùng sau khi đã xác thực xong
                        var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name,user.Name),
                    new Claim(ClaimTypes.Role,role.Name),
                     new Claim(ClaimTypes.NameIdentifier,Convert.ToString(user.Id) ?? "null")
                    };
                        //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme
                        // tạo ra 1 đối tượng xác thực với các thông tin được cung cấp trong danh sach Claim
                        // với 2 tham số là 1 list claims
                        // CookieAuthenticationDefaults.AuthenticationScheme là phương thức xác thực sử dụng Cookie Authentication Scheme
                        // nói cách khác CookieAuthenticationDefaults.AuthenticationScheme cung cấp cookie mặc đinh cho Scheme
                        // cho phép xác thực người dùng bằng cách sử dụng cookie 
                        ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        Console.WriteLine(CookieAuthenticationDefaults.AuthenticationScheme);
                        

                        //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity
                        // Đối tượng ClaimsPrincipal chứa thông tin về người dùng, bao gồm cả đối tượng ClaimsIdentity
                        // và các thông tin khác như tên đăng nhập, mật khẩu, v.v.

                        // => Đối tượng đại diện cho User là ClaimsPricipal,
                        // một ClaimsPrincipal có thể có nhiều ClaimsIdentity chứa các đối tượng Identity cho Authetication Scheme.
                        // Nếu ứng dụng có 1 Scheme thì ClaimsPrincipal sẽ có 1 đối tượng ClaimsIdentity 

                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                        //SignInAsync is a Extension method for Sign in a principal for the specified scheme.

                        //Nếu IsPersistent được thiết lập là true, cookie xác thực sẽ được lưu trữ trên máy khách (client-side cookie)
                        //để cho phép người dùng giữ phiên đăng nhập (persistent session) sau khi đóng trình duyệt hoặc tắt máy tính. Nếu IsPersistent là false,
                        //cookie xác thực sẽ được lưu trữ trong bộ nhớ tạm thời (session cookie), và sẽ bị xóa khi trình duyệt đóng hoặc khi người dùng đăng xuất.
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            principal, new AuthenticationProperties() { IsPersistent = objLoginModel.RememberLogin });

                        return LocalRedirect("/Product");
                    }

                }
                else
                {
                    ViewBag.Message = "Tài Khoản của bạn đã bị khóa vui lòng dùng tài khoản khác";
                }
            }
            return View(objLoginModel);
        }
        [HttpGet]
        public IActionResult CreateLogin()
        {
            if (_context.Roles.Count() == 0)
            {
                Role role = new Role();
                role.Status = 1;
                role.Name = "Other";
                role.Description = "Khách Hàng";
                _context.Roles.Add(role);
                _context.SaveChanges();
            }
         //   ViewData["IdRole"] = new SelectList(_context.Roles, "Id", "Description");
            return View();
        }
        [HttpPost,ActionName("CreateLogin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IdRole,Pass,Status,ConfirmPassword")] SignUp user)
        {
            if(_context.Users.FirstOrDefault(p=>p.Name == user.Name) !=null)
            {
                ViewBag.errorSame = "<p>Tên Tài Khoản Đã Tồn Tại</p>";
                return View(user);
            }

            if (user.Pass != user.ConfirmPassword)
            {
                ViewBag.ConfirmPassword = "<p>Nhập lại mật khẩu không chính xác</p>";
                return View(user);
            }   
             if(user.ConfirmPassword!=null && user.Pass!=(user.ConfirmPassword))
            {
                ViewBag.ConfirmPassword = "<p>Nhập lại mật khẩu không chính xác</p>";
                Console.WriteLine("pass error");
                return View(user);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Cart cart = new Cart();
                    user.Id = Guid.NewGuid();
                    var IdRole = _context.Roles.FirstOrDefault(p => p.Name==("Other"));
                    if(IdRole != null)
                    {
                        user.IdRole = IdRole.Id;
                        _context.Add(new Cart()
                        {
                            IdUser = user.Id,
                            Description = "Giỏ Hàng"
                        });
                    }              
                   
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Login));
                }
            //    ViewData["IdRole"] = new SelectList(_context.Roles, "Id", "Description", user.IdRole);
                return View(user);
            }

        }

        public async Task<IActionResult> LogOut()
        {
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page    
            _serviceCartDetail.RemoveData();
            return LocalRedirect("/");
        }
    }
}
