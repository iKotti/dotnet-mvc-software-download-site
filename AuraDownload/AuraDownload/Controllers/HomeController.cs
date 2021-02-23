using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AuraDownload.Models;

namespace AuraDownload.Controllers
{
    public class HomeController : Controller
    {
        private AuraDownloadEntities db = new AuraDownloadEntities();

        //Index
        public ActionResult Index()
        {
            var kategori = db.category.ToList();
            var program = db.program.Include(p => p.category).ToList();

            KategoriProgramList kpList = new KategoriProgramList();
            kpList.Kategoriler = kategori;
            kpList.Programlar = program;

            return View(kpList);

        }


    }
}