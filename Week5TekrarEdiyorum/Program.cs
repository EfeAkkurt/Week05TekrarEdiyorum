using ConsoleHelpers;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Week5TekrarEdiyorum
{
    internal class Program
    {
        //BU KISIMDA SQL SERVERINA BAĞLANDIK 
        static string connectionString = "";
        static void Main(string[] args)
        {
            //bağlantı kurduk
            using var connection = new SqlConnection(connectionString);

            void MenuGoster()
            {
                Console.Clear();
                int inputSecim = Helper.SecimYaptir(["Öğrencileri Listele", "Öğrenci Ekle", "Öğrenci Düzenle",
                    "Öğrencileri tarihe Göre Sırala", "Sil", "Çıkış"], "Öğrenci Otomasyonu\n".ToUpper());

                switch (inputSecim)
                {
                    case 1:
                        Listele();
                        break;
                    case 2:
                        Ekle();
                        break;
                    case 3:
                        Duzenle();
                        break;
                    case 4:
                        Sil();
                        break;
                    case 5:
                        Cikis();
                        break;
                }
            }

            void MenuyeDon()
            {
                Console.WriteLine("\nMenüye dönmek için bir tuşa basın...");
                Console.ReadKey();
                MenuGoster();
            }

            void Ekle()
            {
                Console.Clear();
                Console.WriteLine("Öğrenci Ekle \n.".ToUpper());
                string inputAd = Helper.BilgiAl("Ad");
                string inputSoyad = Helper.BilgiAl("Soyad");
                string inputEPostaAdresi = Helper.BilgiAl("E-Posta Adresi");
                string inputYas = Helper.BilgiAl("Yas");

                // @ kullan çünkü alt alta yazabiliyorsun bu daha rahat okuma sağlar
                // ad soyad vs bunların başında @ bulunan simge parametredir 
                var sql = @"INSERT INTO 
                Ogrenciler (Ad, Soyad, Yas, EPostaAdresi, OlusturulmaTarihi)
                VALUES (@Ad, @Soyad, @Yas, @EPostaAdresi, @OlusturulmaTarihi)";
                var newStudent = new
                {
                    Ad = inputAd,
                    Soyad = inputSoyad,
                    EPostaAdresi = inputEPostaAdresi,
                    Yas = inputYas,
                    OlusturulmaTarihi = DateTime.Now
                };

                var affRows = connection.Execute(sql, newStudent);

                if (affRows > 0)
                {
                    Helper.Basarili("Öğrenci eklendi.");
                }
                else
                {
                    Helper.Hata("Ekleme yapılamadı!");
                }

                MenuyeDon();
            }

            void Duzenle()
            {
                using var connection = new SqlConnection(connectionString);
                var ogrenciler = connection.Query<Ogrenci>("SELECT * FROM Ogrenciler").ToArray();

                Console.Clear();
                Console.WriteLine("TÜM ÖĞRENCİLER\n");
                //verileri gösterdik
                foreach (var ogrenci in ogrenciler)
                {
                    Console.WriteLine($"{ogrenci.Ad} {ogrenci.Soyad} - {ogrenci.Yas} - {ogrenci.EPostaAdresi} - {ogrenci.OlusturulmaTarihi.ToString("dd.MM.yy HH:mm")}");
                }
                //eğer entere basarsa boş menüye geri döncek
                Console.WriteLine("");
                Console.WriteLine("Menüye dönmek için 'M' tuşuna basınız!");
                if (Console.ReadKey().Key == ConsoleKey.M)
                {
                    return;
                }
                // yeni öğrenciyi giriyor
                int inputId = Helper.SayiAl("Öğrenci Seç.");
                string inputAd = Helper.BilgiAl("Yeni Ad");
                string inputSoyad = Helper.BilgiAl("Yeni Soyad");
                string inputEPostaAdresi = Helper.BilgiAl("Yeni E-Posta Adresi");
                string inputYas = Helper.BilgiAl("Yeni Yas");

                var sql = @"UPDATE Ogrenciler SET Ad = @Ad, Soyad = @Soyad, 
                EPostaAdresi = @EpostaAdresi, Yas = @Yas WHERE Id = @Id";
                var newStudent = new
                {
                    Ad = inputAd,
                    Soyad = inputSoyad,
                    EPostaAdresi = inputEPostaAdresi,
                    Yas = inputYas,
                    OlusturulmaTarihi = DateTime.Now,
                    Id = inputId
                };
                connection.Execute(sql, newStudent);
                Console.WriteLine("Bilgiler Güncelleniyor...");
                Thread.Sleep(1500);
                Helper.RenkliMesajGoster("Güncelleme Başarılı!", ConsoleColor.Green);
                Thread.Sleep(1000);

                MenuyeDon();
            }

            void Listele()
            {
                //We reconnect.
                using var connection = new SqlConnection(connectionString);

                //we want data from students table
                var ogrenciler = connection.Query<Ogrenci>("SELECT * FROM Ogrenciler").ToArray();

                //First cleaning
                Console.Clear();

                //We fetch data with foreach loop
                foreach (var ogrenci in ogrenciler)
                {
                    Console.WriteLine($"{ogrenci.Ad} {ogrenci.Soyad} - {ogrenci.Yas} - {ogrenci.EPostaAdresi} - {ogrenci.OlusturulmaTarihi.ToString("dd.MM.yy HH:mm")}");
                }

                MenuyeDon();
            }

            void Sil()
            {
                using var connection = new SqlConnection(connectionString);
                var ogrenciler = connection.Query<Ogrenci>("SELECT * FROM Ogrenciler").ToArray();
                Console.Clear();
                Console.WriteLine("TÜM ÖĞRENCİLER\n");
                foreach (var ogrenci in ogrenciler)
                {
                    Console.WriteLine($"{ogrenci.Ad} {ogrenci.Soyad} - {ogrenci.Yas} - {ogrenci.EPostaAdresi} - {ogrenci.OlusturulmaTarihi.ToString("dd.MM.yy HH:mm")}");
                }
                Console.WriteLine("");

                int inputId = Helper.SayiAl("Öğrenci Seç");

                var sql = "DELETE FROM Ogrenciler WHERE Id = @Id";

                connection.Execute(sql, new { Id = inputId });

                Console.WriteLine("");
                Helper.Basarili("Öğrenci Silindi!");

                MenuyeDon();
            }

            void TariheGoreListele()
            {
                using var connection = new SqlConnection(connectionString); // CAST ın amacı gruplamak ve içinde hangi değere göre gruplayacağımızı
                                                                            // sölüypurz mesela şuan tarihe göre  yapıyoruz
                                                                            //en eskiden başlayıp istediğin tarihe kadar veya şimdiki tarihe kadar 
                var tarihGruplari = connection.Query<DateTime>(@"
                SELECT VAST (MIN(CreatedDate)as Date) AS TarihGrubu 
                FROM Ogrenciler
                GROUP BY CAST (CreatedDate AS DATE)
                ORDER BY TarihGrubu").ToList(); // verileri aldık grupladık ilk öğrenci eklesin sonrada geri kullanabilisn diye

                if (tarihGruplari.Count == 0) // öğrenci yoksa hata
                {
                    Helper.Hata("Öğrenci Yok!");
                    MenuyeDon();
                    return;
                }

                int currentIndex = 0;
                bool devam = true;
                while (devam)
                {
                    Console.Clear();
                    DateTime seciliTarih = tarihGruplari[currentIndex];
                    Console.WriteLine($"{seciliTarih.ToString("dd MMMM yyyy")}Tarihli Öğrenciler: \n");

                    var ogrenciler = connection.Query<Kisi>("SELECT * FROM Ogrenciler WHERE CAST (Createdate as DATE) = @Tarih ORDER BY CreatedDate", new
                    {
                        Tarih = seciliTarih,
                    }).ToList();

                    if (ogrenciler.Count > 0)
                    {
                        foreach (var ogrenci in ogrenciler)
                        {
                            Console.WriteLine($"{ogrenci.Ad} - {ogrenci.Soyad} - {ogrenci.EPostaAdresi}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Bu tarihte veri yok");
                    }

                    string[] secenekler = { "önceki gün", "sonraki gün", "menüye dön" };

                    int inputSecim = Helper.SecimYaptir(secenekler, "Lütfen Bir seçim yapınız");

                    switch (inputSecim)
                    {
                        case 1:
                            currentIndex--;
                            break;
                        case 2:
                            currentIndex++;
                            break;
                        case 3:
                            devam = false;
                            break;
                        default:
                            Console.WriteLine("Hatalı giriş yaptınız!");
                            break;
                    }

                    MenuyeDon();
                }
            }
            void Cikis()
            {
                Console.Clear();
                Console.WriteLine("Hoşçakalın...");
                Thread.Sleep(1000);
            }

            MenuGoster();
        }
    }
}

