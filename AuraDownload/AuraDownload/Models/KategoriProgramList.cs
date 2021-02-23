using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuraDownload.Models
{
    public class KategoriProgramList
    {
        public List<category> Kategoriler { get; set; }
        public List<program> Programlar { get; set; }
        public program program { get; set; }
        public category kategori { get; set; }
        public List<comment> Yorumlar { get; set; }
        public comment yorum { get; set; }
        public favourite favori { get; set; }
        public List<favourite> Favoriler { get; set; }
        public downloaded indirilen { get; set; }
        public List<downloaded> Indirilenler { get; set; }
    }
}