using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TurApp.Models;
using System.Text;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Transactions;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace TurApp.Controllers.Seguridad
{
    [Authorize]
    public class UsersController : Controller
    {
        private TurAppEntities db = new TurAppEntities();
        private ApplicationDbContext dbUserContext = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: AspNetUsers
        public ActionResult Index()
        {
            return View(db.AspNetUsers.ToList());
        }

        // GET: AspNetUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)//[Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")]
        {
            TurAppEntities Context = new TurAppEntities();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Json(new { ok = "true" });
                    //return RedirectToAction("Index", "AspNetUsers");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }






        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser appUser = new ApplicationUser();
            appUser = UserManager.FindById(id);

            if (appUser == null)
            {
                return HttpNotFound();
            }

            AspNetUsers user = new AspNetUsers();
            user.UserName = appUser.UserName;
            user.FirstName = appUser.FirstName;
            user.LastName = appUser.LastName;
            user.Email = appUser.Email;

            return View(user);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,UserName,FirstName,LastName")] AspNetUsers model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                    //var manager = new UserManager<ApplicationUser>(store);
                    var currentUser = UserManager.FindById(model.Id);
                    currentUser.UserName = model.UserName;
                    currentUser.FirstName = model.FirstName;
                    currentUser.LastName = model.LastName;
                    currentUser.Email = model.Email;
                    await UserManager.UpdateAsync(currentUser);

                    var ctx = store.Context;

                    ctx.SaveChanges();

                    return Json(new { ok = "true" });

                }
                catch (Exception ex)
                {

                    throw ex;
                }



            }
            return View(model);
        }


        // GET: /Account/ResetPassword/id
        [AllowAnonymous]
        public ActionResult ResetPassword(string id)
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);

            ResetPasswordViewModel model = new ResetPasswordViewModel { UserName = aspNetUsers.UserName, Code = "1" };
            return View(model);
        }


        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = UserManager.FindByName(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            model.Code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);//model.Code
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }





        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);

            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUsers);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return Json(new { ok = "true" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }









        // GET: AspNetUsers/SetRoles/5
        public ActionResult SetRoles(string id)
        {
            JsonSerializer jss = new JsonSerializer();

            var user = db.AspNetUsers.FirstOrDefault(r => r.Id == id);//.AspNetRoles.Select(r => r.Id).ToArray();

            //ViewBag.roles = roles;
            ViewBag.RolesID = new SelectList(db.AspNetRoles, "Name", "Name");

            return View(user);
        }

        // POST: AspNetUsers/SetRoles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetRoles(string userId, string[] roles)
        {


            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);

                    var user = UserManager.FindById(userId);

                    //Se eliminan todos los Roles previamente cargados
                    var UserRoles = await UserManager.GetRolesAsync(user.Id);
                    if (UserRoles != null)
                    {
                        await UserManager.RemoveFromRolesAsync(user.Id, UserRoles.ToArray());
                    }

                    //Se agregan los roles seleccionados
                    if (roles.Length>0)
                    {
                        await userManager.AddToRolesAsync(user.Id, roles);
                    }
                    return Json(new
                    {
                        ok = 1,
                        mensaje = ""
                    });

                }




                ////Codigo para Asociar un usuario a un rol
                ////var userStore = new UserStore<ApplicationUser>(context);
                ////var userManager = new UserManager<ApplicationUser>(userStore);

                ////var user = UserManager.FindById(userId);
                ////var user = new ApplicationUser { UserName = "admin" };


                //await userManager.CreateAsync(user);
                //await userManager.AddToRoleAsync(user.Id, "Administrator");

                //return Json(new { ok = "true" });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ok = 0,
                    mensaje = ex.InnerException.Message
                });
                //throw ex;
            }

            
        }
    }
}
