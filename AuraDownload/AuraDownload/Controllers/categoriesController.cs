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
    public class categoriesController : Controller
    {
        private AuraDownloadEntities db = new AuraDownloadEntities();
        
        //Kategori Detay
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                RedirectToAction("index", "Home");
            }

            KategoriProgramList kplist = new KategoriProgramList();
            kplist.kategori = db.category.Find(id);
            kplist.Kategoriler = db.category.ToList();
            kplist.Programlar = db.program.ToList();
            kplist.Favoriler = db.favourite.ToList();

            return View(kplist);
        }
    }
}