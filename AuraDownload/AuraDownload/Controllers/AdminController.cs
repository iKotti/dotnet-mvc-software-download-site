using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AuraDownload.Models;
using AuraDownload.Controllers;

namespace AuraDownload.Controllers
{
    public class AdminController : Controller
    {
        private AuraDownloadEntities db = new AuraDownloadEntities();

        //Index
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                if (Convert.ToInt32(Session["usertype"]) == 4)
                {
                    return RedirectToAction("kullanicilar", "admin");
                }
                else
                {
                    TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> Bu sayfaya gitme yetkiniz yok </div>";
                    return RedirectToAction("index", "Home");
                }

            }
            else
            {
                TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;margin-left:auto;margin-right:auto;'> Lütfen giriş yapınız </div>";
                return RedirectToAction("login", "users");
            }
        }


        //Kullanıcılar
        public ActionResult kullanicilar()
        {
            if (Session["username"] != null)
            {
                if (Convert.ToInt32(Session["usertype"]) == 4)
                {
                    return View(db.user.ToList());
                }
                else
                {
                    TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> Bu sayfaya gitme yetkiniz yok </div>";
                    return RedirectToAction("index", "Home");
                }

            }
            else
            {
                TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;margin-left:auto;margin-right:auto;'> Lütfen giriş yapınız </div>";
                return RedirectToAction("login", "users");
            }

        }
        public ActionResult deleteUser(int id)
        {
            var gelenKullanici = db.user.Find(id);
            db.user.Remove(gelenKullanici);
            db.SaveChanges();
            TempData["alert"] = "<div class='alert alert-success' style='margin-top:10px;width:20%;border:1px solid'> <strong>Başarılı!</strong> Kullanıcı Silindi </div>";
            return RedirectToAction("kullanicilar", "admin");
        }


        //Kategoriler
        public ActionResult kategoriler()
        {
            if (Session["username"] != null)
            {
                if (Convert.ToInt32(Session["usertype"]) == 4)
                {
                    return View(db.category.ToList());
                }
                else
                {
                    TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> Bu sayfaya gitme yetkiniz yok </div>";
                    return RedirectToAction("index", "Home");
                }

            }
            else
            {
                TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;margin-left:auto;margin-right:auto;'> Lütfen giriş yapınız </div>";
                return RedirectToAction("login", "users");
            }
        }
        public ActionResult deleteCategory(int id)
        {
            category category = db.category.Find(id);
            db.category.Remove(category);
            db.SaveChanges();
            TempData["alert"] = "<div class='alert alert-success' style='margin-top:10px;width:20%;border:1px solid'> <strong>Başarılı!</strong> Kategori Silindi </div>";
            return RedirectToAction("kategoriler", "admin");
        }
        public ActionResult addCategory()
        {
            if (Session["username"] != null)
            {
                if (Convert.ToInt32(Session["usertype"]) == 4)
                {
                    return View();
                }
                else
                {
                    TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> Bu sayfaya gitme yetkiniz yok </div>";
                    return RedirectToAction("index", "Home");
                }

            }
            else
            {
                TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;margin-left:auto;margin-right:auto;'> Lütfen giriş yapınız </div>";
                return RedirectToAction("login", "users");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addCategory(category category)
        {
            category varMi = db.category.Where(c => c.category_name == category.category_name).FirstOrDefault();

            if (ModelState.IsValid && varMi == null)
            {
                db.category.Add(category);
                db.SaveChanges();
                TempData["alert"] = "<div class='alert alert-success' style='margin-top:10px;width:20%;border:1px solid'> <strong>Başarılı!</strong> Kategori Eklendi </div>";
                return RedirectToAction("kategoriler", "admin");
            }
            else
            {
                TempData["alert"] = "<div class='alert alert-warning' style='margin-top:10px;width:20%;border:1px solid'> <strong>Hata!</strong> Kategori Mevcut </div>";
                return RedirectToAction("kategoriler", "admin");
            }


        }
        public ActionResult editCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editCategory(category category)
        {
            if (ModelState.IsValid)
            {
                category varMi = db.category.Where(c => c.category_name == category.category_name).FirstOrDefault();
                if (varMi == null)
                {
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["alert"] = "<div class='alert alert-success' style='margin-top:10px;width:20%;border:1px solid'> <strong>Başarılı!</strong> Kategori Güncellendi </div>";
                    return RedirectToAction("kategoriler", "Admin");
                }

                else
                {
                    TempData["alert"] = "<div class='alert alert-warning' style='margin-top:10px;width:20%;border:1px solid'> <strong>Hata!</strong> Kategori Mevcut </div>";
                    return RedirectToAction("kategoriler", "Admin");
                }

            }
            TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;width:20%;border:1px solid'> <strong>Hata!</strong> Kategori Adı Girmediniz </div>";
            return RedirectToAction("kategoriler", "Admin");
        }

        //Yorumlar
        public ActionResult yorumlar()
        {
            if (Session["username"] != null)
            {
                if (Convert.ToInt32(Session["usertype"]) == 4)
                {
                    return View(db.comment.ToList());
                }
                else
                {
                    TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> Bu sayfaya gitme yetkiniz yok </div>";
                    return RedirectToAction("index", "Home");
                }

            }
            else
            {
                TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;margin-left:auto;margin-right:auto;'> Lütfen giriş yapınız </div>";
                return RedirectToAction("login", "users");
            }
        }
        public ActionResult deleteComment(int id)
        {
            comment comment = db.comment.Find(id);
            db.comment.Remove(comment);
            db.SaveChanges();
            TempData["alert"] = "<div class='alert alert-success' style='margin-top:10px;width:20%;border:1px solid'> <strong>Başarılı!</strong> Yorum Silindi </div>";
            return RedirectToAction("yorumlar", "admin");
        }

        //Programlar
        public ActionResult programlar()
        {
            if (Session["username"] != null)
            {
                if (Convert.ToInt32(Session["usertype"]) == 4)
                {
                    return View(db.program.ToList());
                }
                else
                {
                    TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> Bu sayfaya gitme yetkiniz yok </div>";
                    return RedirectToAction("index", "Home");
                }

            }
            else
            {
                TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;margin-left:auto;margin-right:auto;'> Lütfen giriş yapınız </div>";
                return RedirectToAction("login", "users");
            }
        }
        public ActionResult deleteProgram(int? id)
        {
            program program = db.program.Find(id);
            List<favourite> rmvFav = db.favourite.Where(f => f.program_id == id).ToList();
            List<comment> rmvCom = db.comment.Where(c => c.program_id == id).ToList();
            db.program.Remove(program);
            if (rmvFav != null)
            {
                foreach (var fav in rmvFav)
                {
                    db.favourite.Remove(fav);
                }
            }
            if (rmvCom != null)
            {
                foreach (var com in rmvCom)
                {
                    db.comment.Remove(com);
                }
            }

            db.SaveChanges();
            TempData["alert"] = "<div class='alert alert-success' style='margin-top:10px;width:20%;border:1px solid'> <strong>Başarılı!</strong> Program Silindi </div>";
            return RedirectToAction("programlar", "admin");
        }
        public ActionResult addProgram()
        {
            ViewBag.category_id = new SelectList(db.category, "category_id", "category_name");
            ViewBag.user_id = new SelectList(db.user, "user_id", "name_surname");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addProgram(program program)
        {
            if (ModelState.IsValid)
            {
                program.user_id = Convert.ToInt32(Session["user_id"]);
                program.download_count = 0;
                Session["program_id"] = program.program_id;
                db.program.Add(program);
                db.SaveChanges();
                TempData["alert"] = "<div class='alert alert-success' style='margin-top:10px;width:20%;border:1px solid'> <strong>Başarılı!</strong> Program Eklendi </div>";
                return RedirectToAction("programlar", "admin");
            }

            ViewBag.category_id = new SelectList(db.category, "category_id", "category_name", program.category_id);
            ViewBag.user_id = new SelectList(db.user, "user_id", "name_surname", program.user_id);
            return View(program);
        }
    }

}