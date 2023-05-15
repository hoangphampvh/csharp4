using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using assiment_csad4.Configruration;
using assiment_csad4.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using assiment_csad4.ViewModel;
using assiment_csad4.IService;
using assiment_csad4.Service;

namespace assiment_csad4.Controllers
{
    public class UserController : Controller
    {
        private readonly MyDbContext _context;
        private IServiceUser serviceUser;
        public UserController(UserService userService)
        {
            serviceUser = userService;
            _context = new MyDbContext();
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.Users.Include(u => u.RoleNavigation);
            return View(await myDbContext.ToListAsync());
        }
        public async Task<IActionResult> IndexUser()
        {
            var myDbContext = _context.Users.Include(u => u.RoleNavigation).Where(p=>p.RoleNavigation.Name.Contains("Other"));
            return View(await myDbContext.ToListAsync());
        }
        // GET: User/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.RoleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
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
            ViewData["IdRole"] = new SelectList(_context.Roles, "Id", "Description");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Pass,IdRole,Status,ConfirmPassword")] SignUp user)
        {
            if(_context.Roles.Count() == 0)
            {
                Role role = new Role();
                role.Status = 1;
                role.Name = "Other";
                role.Description = "Khách Hàng";
                _context.Roles.Add(role);
                _context.SaveChanges();
            }
           

            if (_context.Users.FirstOrDefault(p => p.Name == user.Name) != null)
            {
                ViewBag.errorSame = "<p>Tên đã có người khác sử dụng</p> <p>Vui lòng đặt tên mới</p>";
                return View(user);
            }
            else if (user.Pass != user.ConfirmPassword)
            {
                ViewBag.ConfirmPassword = "<p>Nhập lại mật khẩu không chính xác</p>";
                return View(user);
            }   
            else
            {
                if (ModelState.IsValid)
                {
                    Console.WriteLine(user.Pass);
                    Console.WriteLine(user.ConfirmPassword);
                    user.Id = Guid.NewGuid();
                    user.Id = Guid.NewGuid();
                        _context.Add(new Cart()
                        {
                            IdUser = user.Id,
                            Description = "Giỏ Hàng"
                        });
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdRole"] = new SelectList(_context.Roles, "Id", "Description", user.IdRole);
                return View(user);
            }

        }
        // GET: User/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var data = new object[]
        {
                 new
                {
                    value =0,
                    status = "Đang Hoạt Động"
                },
                new
                {
                     value =1,
                     status = "Không Hoạt Động"
                }
        };
            ViewData["listStatus"] = new SelectList(data, "value", "status");
            ViewData["IdRole"] = new SelectList(_context.Roles, "Id", "Description", user.IdRole);
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Name,Pass,IdRole,Status")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    serviceUser.UpdateUser(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
                    value =0,
                    status = "Đang Hoạt Động"
                },
                new
                {
                     value =1,
                     status = "Không Hoạt Động"
                }
        };
            ViewData["listStatus"] = new SelectList(data, "value", "status");
            ViewData["IdRole"] = new SelectList(_context.Roles, "Id", "Description", user.IdRole);
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.RoleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'MyDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(Guid id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }            
    }
}
