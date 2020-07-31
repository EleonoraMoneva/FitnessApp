using System.Linq;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace FitnessApp.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private IPasswordValidator<AppUser> passwordValidator;
        private IUserValidator<AppUser> userValidator;
        private readonly FitnessAppContext _context;
        public AdminController(UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> passwordHash, IPasswordValidator<AppUser> passwordVal, IUserValidator<AppUser>
 userValid,FitnessAppContext context)
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
            passwordValidator = passwordVal;
            userValidator = userValid;
            _context = context;
        }
        public IActionResult Index()
        {
            IQueryable<AppUser> users = userManager.Users.OrderBy(u => u.Email);
            return View(users);
        }
        public IActionResult TrainerProfile(int trainerId)
        {
            //AppUser user = await userManager.FindByIdAsync(id);
            AppUser user = userManager.Users.FirstOrDefault(u => u.TrainerId == trainerId);
            Trainer trainer = _context.Trainer.Where(s => s.Id == trainerId).FirstOrDefault();
            if (trainer != null)
            {
                ViewData["Name"] = trainer.FullName;
                ViewData["TrainerId"] = trainer.Id;
            }
            if (user != null)
                return View(user);
            else
                return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> TrainerProfile(int trainerId, string email, string password, string phoneNumber)
        {
            //AppUser user = await userManager.FindByIdAsync(id);
            AppUser user = userManager.Users.FirstOrDefault(u => u.TrainerId == trainerId);
            if (user != null)
            {
                IdentityResult validUser = null;
                IdentityResult validPass = null;

                user.Email = email;
                user.UserName = email;
                user.PhoneNumber = phoneNumber;

                if (string.IsNullOrEmpty(email))
                    ModelState.AddModelError("", "Email cannot be empty");

                validUser = await userValidator.ValidateAsync(userManager, user);
                if (!validUser.Succeeded)
                    Errors(validUser);

                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPass.Succeeded)
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                    else
                        Errors(validPass);
                }

                if (validUser != null && validUser.Succeeded && (string.IsNullOrEmpty(password) || validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(TrainerProfile), new { trainerId });
                    else
                        Errors(result);
                }
            }
            else
            {
                AppUser newuser = new AppUser();
                IdentityResult validUser = null;
                IdentityResult validPass = null;

                newuser.Email = email;
                newuser.UserName = email;
                newuser.PhoneNumber = phoneNumber;
                newuser.TrainerId = trainerId;
                newuser.Role = "Trainer";

                if (string.IsNullOrEmpty(email))
                    ModelState.AddModelError("", "Email cannot be empty");

                validUser = await userValidator.ValidateAsync(userManager, newuser);
                if (!validUser.Succeeded)
                    Errors(validUser);

                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, newuser, password);
                    if (validPass.Succeeded)
                        newuser.PasswordHash = passwordHasher.HashPassword(newuser, password);
                    else
                        Errors(validPass);
                }
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (validUser != null && validUser.Succeeded && validPass != null && validPass.Succeeded)
                {
                    IdentityResult result = await userManager.CreateAsync(newuser, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newuser, "Trainer");
                        return RedirectToAction(nameof(TrainerProfile), new { trainerId });
                    }
                    else
                        Errors(result);
                }
                user = newuser;
            }
            Trainer trainer = _context.Trainer.Where(s => s.Id == trainerId).FirstOrDefault();
            if (trainer != null)
            {
                ViewData["Name"] = trainer.FullName;
                ViewData["TrainerId"] = trainer.Id;
            }
            return View(user);
        }
        public IActionResult UserProfile(int userId)
        {
            //AppUser user = await userManager.FindByIdAsync(id);
            AppUser user = userManager.Users.FirstOrDefault(u => u.UserId == userId);
            User user1 = _context.User.Where(s => s.Id == userId).FirstOrDefault();
            if (user1 != null)
            {
                ViewData["FullName"] = user1.FullName;
                ViewData["UserId"] = user1.Id;
            }
            if (user != null)
                return View(user);
            else
                return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> UserProfile(int userId, string email, string password, string phoneNumber)
        {
            //AppUser user = await userManager.FindByIdAsync(id);
            AppUser user = userManager.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                IdentityResult validUser = null;
                IdentityResult validPass = null;

                user.Email = email;
                user.UserName = email;
                user.PhoneNumber = phoneNumber;

                if (string.IsNullOrEmpty(email))
                    ModelState.AddModelError("", "Email cannot be empty");

                validUser = await userValidator.ValidateAsync(userManager, user);
                if (!validUser.Succeeded)
                    Errors(validUser);

                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPass.Succeeded)
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                    else
                        Errors(validPass);
                }

                if (validUser != null && validUser.Succeeded && (string.IsNullOrEmpty(password) || validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(UserProfile), new { userId });
                    else
                        Errors(result);
                }
            }
            else
            {
                AppUser newuser = new AppUser();
                IdentityResult validUser = null;
                IdentityResult validPass = null;

                newuser.Email = email;
                newuser.UserName = email;
                newuser.PhoneNumber = phoneNumber;
                newuser.UserId = userId;
                newuser.Role = "User";

                if (string.IsNullOrEmpty(email))
                    ModelState.AddModelError("", "Email cannot be empty");

                validUser = await userValidator.ValidateAsync(userManager, newuser);
                if (!validUser.Succeeded)
                    Errors(validUser);

                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, newuser, password);
                    if (validPass.Succeeded)
                        newuser.PasswordHash = passwordHasher.HashPassword(newuser, password);
                    else
                        Errors(validPass);
                }
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (validUser != null && validUser.Succeeded && validPass != null && validPass.Succeeded)
                {
                    IdentityResult result = await userManager.CreateAsync(newuser, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newuser, "User");
                        return RedirectToAction(nameof(UserProfile), new { userId });
                    }
                    else
                        Errors(result);
                }
                user = newuser;
            }
            User user1 = _context.User.Where(s => s.Id == userId).FirstOrDefault();
            if (user1 != null)
            {
                ViewData["FullName"] = user1.FullName;
                ViewData["UserId"] = user1.Id;
            }
            return View(user);
        }
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

    }
}
