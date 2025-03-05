using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim
{
    internal class Kullanici
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public DateTime Olusturulmatarihi { get; set; }
        public DateTime GüncellenmeTarihi { get; set; }

    }
}