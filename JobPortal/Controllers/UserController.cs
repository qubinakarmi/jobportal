using JobPortal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



namespace JobPortal.Controllers
{
    public class UserController : Controller
    {
        JobPortalDbContext db_context = new JobPortalDbContext();
        public ActionResult Register()
        {
            return View();
        }
        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserViewModel uvm)
        {
            try
            {
                var user_exists = db_context.Users.Any(x => x.UserName == uvm.UserName);
                if (!user_exists)
                {
                    var entity = new User()
                    {
                        UserName = uvm.UserName,
                        Password = uvm.Password,
                    };
                    db_context.Users.Add(entity);
                    db_context.SaveChanges();

                    return RedirectToAction(nameof(Login));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Details/5
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserViewModel vm)
        {

            var db_user = db_context.Users.Where(x =>
                x.UserName == vm.UserName).FirstOrDefault();
            if (db_user != null && db_user.Password == vm.Password)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, db_user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, db_user.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity( claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties();

                HttpContext.SignInAsync( 
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                    );
                return RedirectToAction("Index", "Job");

            }
            return View();
        }

        // GET: UserController/Create
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View("Login");
        }

        //dotnet add package Microsoft.AspNetCore.Authentication.Cookies


    }
}
