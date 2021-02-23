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
    public class programsController : Controller
    {
        private AuraDownloadEntities db = new AuraDownloadEntities();

        //Index
        public ActionResult Index()
        {

            KategoriProgramList kpList = new KategoriProgramList
            {
                Kategoriler = db.category.ToList(),
                Programlar = db.program.Include(p => p.category).ToList()
            };

            return View(kpList);
        }


        //Program Details
        public ActionResult Details(int? id)
        {
            KategoriProgramList kplist = new KategoriProgramList();

            if (Session["username"] != null)
            {
                kplist.Yorumlar = db.comment.Where(c => c.program_id == id).ToList();
                kplist.program = db.program.Find(id);
                kplist.Kategoriler = db.category.ToList();
                kplist.Programlar = db.program.ToList();
                kplist.Favoriler = db.favourite.ToList();
            }
            else
            {

                TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;margin-left:auto;margin-right:auto;'> Lütfen giriş yapınız... </div>";
                return RedirectToAction("Login", "users");

            }

            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if(kplist.program == null)
            {
                return RedirectToAction("Index", "Home");
            }


            return View(kplist);
        }
        [HttpPost]
        public ActionResult Details(string comment, int id)
        {
            try
            {
                comment yorum = new comment();
                yorum.user_id = Convert.ToInt32(Session["user_id"]);
                yorum.comment_content = comment;
                yorum.program_id = id;
                yorum.comment_time = DateTime.Now;
                db.comment.Add(yorum);
                db.SaveChanges();
                TempData["alert"] = "<div class='alert alert-success' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> Yorumunuz başarıyla kaydedildi ! </div>";
            }

            catch (Exception e)
            {
                TempData["YorumBildirimi"] = "<div class='alert alert-danger' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> Yorum kaydedilemedi ! </div>";
            }

            return RedirectToAction("Details", "programs", new { id = id });
        }


        //Create Program
        public ActionResult Create()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "users");
            }

            ViewBag.category_id = new SelectList(db.category, "category_id", "category_name");
            ViewBag.user_id = new SelectList(db.user, "user_id", "name_surname");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(program program)
        {
            if (ModelState.IsValid)
            {
                program.user_id = Convert.ToInt32(Session["user_id"]);
                program.download_count = 0;
                Session["program_id"] = program.program_id;
                db.program.Add(program);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(db.category, "category_id", "category_name", program.category_id);
            ViewBag.user_id = new SelectList(db.user, "user_id", "name_surname", program.user_id);
            return View(program);
        }


        //Download Program
        public ActionResult linkTiklandi64(int id)
        {
            var program = db.program.Where(p => p.program_id == id).FirstOrDefault();
            program.download_count++;

            List<downloaded> indirilenler = db.downloaded.ToList();

            downloaded varMi = indirilenler.Where(i => i.program_id == id && i.user_id == Convert.ToInt32(Session["user_id"])).FirstOrDefault();

            if (varMi == null)
            {
                var downloaded = new downloaded();
                downloaded.program_id = program.program_id;
                downloaded.user_id = Convert.ToInt32(Session["user_id"]);
                db.downloaded.Add(downloaded);
                db.SaveChanges();
            }
            db.SaveChanges();
            return Redirect(program.link64);
        }
        public ActionResult linkTiklandi32(int id)
        {
            var program = db.program.Where(p => p.program_id == id).FirstOrDefault();
            program.download_count++;

            List<downloaded> indirilenler = db.downloaded.ToList();

            downloaded varMi = indirilenler.Where(i => i.program_id == id && i.user_id == Convert.ToInt32(Session["user_id"])).FirstOrDefault();

            if (varMi == null)
            {
                var downloaded = new downloaded();
                downloaded.program_id = program.program_id;
                downloaded.user_id = Convert.ToInt32(Session["user_id"]);
                db.downloaded.Add(downloaded);
                db.SaveChanges();
            }

            db.SaveChanges();
            return Redirect(program.link32);
        }


        //Favourite Program
        public ActionResult Favourite()
        {
            if (Session["username"] != null)
            {
                KategoriProgramList kpList = new KategoriProgramList();
                kpList.Kategoriler = db.category.ToList();
                kpList.Favoriler = db.favourite.ToList();
                kpList.Programlar = db.program.ToList();
                return View(kpList);
            }
            else
            {
                TempData["alert"] = "<div class='alert alert-success' style='margin-top:10px;margin-left:auto;margin-right:auto;'> Lütfen giriş yapınız... </div>";
                return RedirectToAction("Login", "users");
            }
        }
        public ActionResult addFavourite(int id)
        {
            List<favourite> Begenilenler = db.favourite.ToList();

            favourite varMi = Begenilenler.Where(i => i.program_id == id && i.user_id == Convert.ToInt32(Session["user_id"])).FirstOrDefault();

            if (varMi == null)
            {
                var favourite = new favourite();
                favourite.program_id = id;
                favourite.user_id = Convert.ToInt32(Session["user_id"]);
                db.favourite.Add(favourite);
                db.SaveChanges();
                TempData["alert"] = "<div class='alert alert-success' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> <strong> Başarılı ! </strong> Program favorilerinize eklendi.. </div>";
            }

            db.SaveChanges();
            return RedirectToAction("details","programs",new {id=id });
        }
        public ActionResult deleteFavourite(int id)
        {
            favourite gelenProgram = db.favourite.Where(f => f.program_id == id).FirstOrDefault();

            if (gelenProgram != null)
            {
                db.favourite.Remove(gelenProgram);
                db.SaveChanges();
                TempData["alert"] = "<div class='alert alert-success' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> <strong> Başarılı ! </strong> Program favorilerinizden silindi... </div>";
                return RedirectToAction("Favourite", "programs");
            }
            else
            {
                TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> <strong> Hata ! </strong> Program favorilerinizden silinemedi. Bu program zaten favorilerinizde olmayabilir mi ? </div>";
                return RedirectToAction("Favourite", "programs");
            }
        }


        //Downloaded Program
        public ActionResult Downloaded()
        {

            if (Session["username"] != null)
            {
                KategoriProgramList kpList = new KategoriProgramList();
                kpList.Programlar = db.program.ToList();
                kpList.Kategoriler = db.category.ToList();
                kpList.Indirilenler = db.downloaded.ToList();
                return View(kpList);
            }
            else
            {
                TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;margin-left:auto;margin-right:auto;'> Lütfen giriş yapınız... </div>";
                return RedirectToAction("Login", "users");
            }
        }
        public ActionResult deleteDownloaded(int id)
        {
            downloaded gelenProgram = db.downloaded.Where(f => f.program_id == id).FirstOrDefault();

            if (gelenProgram != null)
            {
                db.downloaded.Remove(gelenProgram);
                db.SaveChanges();
                TempData["alert"] = "<div class='alert alert-success' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> <strong> Başarılı ! </strong> Program indirilenler listesinden silindi... </div>";
                return RedirectToAction("Downloaded", "programs");
            }
            else
            {
                TempData["alert"] = "<div class='alert alert-danger' style='margin-top:10px;max-width:60%;margin-left:auto;margin-right:auto;'> <strong> Hata ! </strong> Program indirilenler listesinden silinemedi. Bu program zaten listeniz de olmayabilir mi ? </div>";
                return RedirectToAction("Downloaded", "programs");
            }
        }
    }
}
