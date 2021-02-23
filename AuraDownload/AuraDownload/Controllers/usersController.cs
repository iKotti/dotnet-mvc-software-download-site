using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AuraDownload.Models;
using System.Security.Cryptography;

namespace AuraDownload.Controllers
{
    public class usersController : Controller
    {
        private AuraDownloadEntities db = new AuraDownloadEntities();

        //Login
        public ActionResult Login()
        {
            if (Session["username"] != null) { return RedirectToAction("index", "home"); }
            else { return View(); }
        }
        [HttpPost]
        public ActionResult Login(user model)
        {
            KategoriProgramList kpList = new KategoriProgramList();

            SHA1 sha = new SHA1CryptoServiceProvider();
            var pass = model.password;
            string sifrelenmisSifre = Convert.ToBase64String(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass)));

            var userGiris = db.user.FirstOrDefault(i => i.username == model.username && i.password == sifrelenmisSifre);
            if (userGiris != null)
            {
                Session["user_id"] = userGiris.user_id;
                Session["username"] = userGiris.username;
                Session["usertype"] = userGiris.user_type;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;'> Kullanıcı adı veya şifre hatalı ! </div>";
            }
            var kullaniciadi = model.username;
            ViewBag.Username = kullaniciadi;
            return View();
        }


        //Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(user user,string confirmPassword)
        {
            var usernameVarMi = db.user.Where(u => u.username == user.username).FirstOrDefault();
            var mailVarMi = db.user.Where(u => u.mail == user.mail).FirstOrDefault();

            if(user.password == confirmPassword)
            {
                if (ModelState.IsValid && usernameVarMi == null && mailVarMi == null)
                {
                    SHA1 sha = new SHA1CryptoServiceProvider();
                    var pass = user.password;
                    string sifrelenmisSifre = Convert.ToBase64String(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass)));
                    user.password = sifrelenmisSifre;
                    user.user_type = 1;
                    db.user.Add(user);
                    db.SaveChanges();
                    TempData["alert"] = "<div class='alert alert-success' style='margin-top:10px;margin-left:auto;margin-right:auto;'> Kaydınız başarılı </div>";
                    return RedirectToAction("Login", "users");
                }

                else if (usernameVarMi != null && mailVarMi == null)
                {
                    TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;margin-left:auto;margin-right:auto;'> Kayıtlı kullanıcı adı</div>";
                    return RedirectToAction("Register", "users");
                }

                else if (usernameVarMi == null && mailVarMi != null)
                {
                    TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;margin-left:auto;margin-right:auto;'> Kayıtlı mail adresi </div>";
                    return RedirectToAction("Register", "users");
                }

                else if (usernameVarMi != null && mailVarMi != null)
                {
                    TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;margin-left:auto;margin-right:auto;'> Kayıtlı kullanıcı adı ve mail adresi</div>";
                    return RedirectToAction("Register", "users");
                }

                return View(user);
            }
            else
            {
                TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;margin-left:auto;margin-right:auto;'> Şifreler uyuşmuyor </div>";
                return RedirectToAction("Register", "users");
            }
            
        }

        public ActionResult Register()
        {
            if(Session["username"] != null) { return RedirectToAction("index", "home"); }
            else { return View(); }
           
        }


        //Logout
        public ActionResult Logout()
        {
            try
            {
                Session["username"] = null;
                TempData["alert"] = "<div class='alert alert-success' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> Çıkış başarılı... </div>";
            }
            catch(Exception e)
            {
                TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> Çıkış başarısız... </div>";
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
