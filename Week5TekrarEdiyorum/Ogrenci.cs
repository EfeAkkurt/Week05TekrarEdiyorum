using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week5TekrarEdiyorum
{
    public class Ogrenci
    {
        public int Id { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? EPostaAdresi { get; set; }
        public int Yas { get; set; }
        public DateTime OlusturulmaTarihi { get; set; }
    }
    public class Kisi
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string EPostaAdresi { get; set; }
        public string Yas { get; set; }
    }
}

