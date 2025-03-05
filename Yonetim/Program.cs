using Dapper;
using Microsoft.Data.SqlClient;

namespace Yonetim
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=USER;Database=deneme;Integrated Security=true;TrustServerCertificate=True";
            // using kelimesi sayesinde bu bir ifade şekli 
            // using amacı kapıyı aç işi bitince kapat
            // sürekli açık kalmıyor bu sayede
            // işlemi aç kapat
            using var connection = new SqlConnection(connectionString);
            //queryFirst çoklu bir ifadeden ilkini getir demek
            //querySingle ise tek bir sonuç almaya odaklı
            //singleordefault ise sen ilk önce bak ilk önce sonuç alabiliyor mu alamazsan eğer patlat çalışmazsa null yani
            //var sql = "SELECT * FROM Kullanici WHERE KullaniciAdi = @kullaniciAdi AND Sifre = @sifre ";
            //var kullanici = connection.QuerySingleOrDefault<Kullanici>(sql, new {kullaniciAdi = "Bayırdomuzu81", sifre = "trakyaşarapçısı12" });

            //if (kullanici != null)
            //{
            //    Console.WriteLine($"Merhaba {kullanici.Ad}");
            //}else
            //{
            //    Console.WriteLine("Eksik veya Hatalı giriş yaptınız!");
            //}
            //Console.WriteLine(kullanici);

            //KULLANICI VAR MI YOK MU ?

            var sql = "SELECT COUNT(KullaniciAdi) as KullaniciSayisi FROM Kullanici WHERE KullaniciAdi = @kullaniciAdi";
            var kullaniciVarMi = connection.ExecuteScalar<bool>(sql, new { KullaniciAdi = "Bayırdomuzu81" });
            if(kullaniciVarMi)
            {
                // kullanıcı adı müsait ekleyebilirsin
                Console.WriteLine("Ekle");
            }else
            {
                // HATA kullanıcı adı mevcut
                Console.WriteLine("Hata kullanıcı adı mevcut");
            }
            Kullanici? loggedInUser = null;
            //loggedInUser = user
            //loggedInUser = girisYapanKullanici
        }
    }
}
