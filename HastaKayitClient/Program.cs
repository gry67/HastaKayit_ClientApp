using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HastaKayitClient
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            bool cikis = true;
            while (cikis)
            {
                Console.WriteLine("Hoşgeldiniz... Ne yapmak istersiniz ? sayı girerek seçin");
                Console.WriteLine("1-)Hasta Kayıt Ekle \n" +
                    "2-)Bütün Kayıtları Göster\n" +
                    "3-)HastaId'ye Göre Kayıt Sorgula\n" +
                    "4-)Kayıt Güncelle\n" +
                    "5-)Id'ye göre Kayıt sil\n" +
                    "6-)Çıkış yap");
                HttpClient client = new HttpClient();
                string url = "https://localhost:7122/api/hasta";
                byte secim = byte.Parse(Console.ReadLine());

                List<Hasta> json_list = null;

                switch (secim)
                {
                    case 1:
                        Console.WriteLine("Bilgilerini eklemek istediğiniz hastanın ismini giriniz...");
                        string ad = Console.ReadLine();
                        Console.WriteLine("Bilgilerini eklemek istediğiniz hastanın soyadını giriniz...");
                        string soyad = Console.ReadLine();
                        Console.WriteLine("Bilgilerini eklemek istediğiniz hastanın Tc.Kimlik No'sunu giriniz...");
                        string kimlikNo = Console.ReadLine();

                        Hasta hasta = new Hasta()
                        {
                            hastaAdi = ad,
                            hastaSoyadi = soyad,
                            hastaKimlikNo = kimlikNo
                        };

                        string json_Body = JsonConvert.SerializeObject(hasta, Formatting.Indented);
                        HttpContent content = new StringContent(json_Body,Encoding.UTF8, "application/json");
                        HttpResponseMessage responseMessage = await client.PostAsync(url, content);
                        
                        Console.WriteLine(responseMessage.StatusCode + " " + responseMessage.Content);
                        
                        string responseContent = await responseMessage.Content.ReadAsStringAsync();
                        
                        Console.WriteLine(responseContent); // Yanıt içeriğini yazdırma

                        break;
                    case 2:

                        HttpResponseMessage message = await client.GetAsync(url);
                        string icerik = await message.Content.ReadAsStringAsync();
                        json_list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Hasta>>(icerik);

                        foreach (var item in json_list)
                        {
                            Console.WriteLine("\nHasta ID: " + item.hastaId + "\nHasta Adı: " +
                                item.hastaAdi + "\nHasta Soyadı: " + item.hastaSoyadi + "\nHasta Kimlik No:" + item.hastaKimlikNo);
                        }Console.WriteLine();
                        break;

                    case 3:
                        Console.WriteLine("Sorgulamak istediğiniz hastanın id'sini giriniz...");
                        int sorgu_id = int.Parse(Console.ReadLine());
                        string yeni_url = $"{url}/{sorgu_id}";

                        HttpResponseMessage Sorgu_message = await client.GetAsync(yeni_url);

                        string Sorgu_res_icerik = await Sorgu_message.Content.ReadAsStringAsync();

                        Hasta sorgu_h = JsonConvert.DeserializeObject<Hasta>(Sorgu_res_icerik);

                        Console.WriteLine(sorgu_h.hastaId+"\n"+ sorgu_h.hastaAdi+"\n"+ sorgu_h.hastaSoyadi+"\n"+ sorgu_h.hastaKimlikNo);
                        Console.WriteLine();
                        break;

                    case 4:
                        Console.WriteLine("Bilgilerini değiştirmek istediğiniz hastanın id'sini giriniz...");
                        int put_id = int.Parse(Console.ReadLine());
                        Console.WriteLine("Bilgilerini değiştirmek istediğiniz hastanın ismini giriniz...");
                        string put_ad = Console.ReadLine();
                        Console.WriteLine("Bilgilerini değiştirmek istediğiniz hastanın soyadını giriniz...");
                        string put_soyad = Console.ReadLine();
                        Console.WriteLine("Bilgilerini değiştirmek istediğiniz hastanın Tc.Kimlik No'sunu giriniz...");
                        string put_kimlikNo = Console.ReadLine();

                        Hasta put_hasta = new Hasta()
                        {
                            hastaId = put_id,
                            hastaAdi = put_ad,
                            hastaSoyadi = put_soyad,
                            hastaKimlikNo = put_kimlikNo
                        };
                        string put_json_Body = JsonConvert.SerializeObject(put_hasta, Formatting.Indented);
                        Console.WriteLine(put_json_Body);
                        HttpContent put_content = new StringContent(put_json_Body, Encoding.UTF8, "application/json");
                        HttpResponseMessage put_responseMessage = await client.PutAsync(url, put_content);
                        Console.WriteLine(put_responseMessage);

                        break;

                    case 5: Console.WriteLine("Kaydını silmek istediğiniz hastanın id'sini giriniz...");
                            int delete_id=int.Parse(Console.ReadLine());
                            string yeni_delete_url = $"{url}/{delete_id}";
                            HttpResponseMessage delete_responseMessage = await client.DeleteAsync(yeni_delete_url);
                            string delete_content = await delete_responseMessage.Content.ReadAsStringAsync();
                            Console.WriteLine(delete_content+"\n");

                        break;
                    case 6: cikis = false; break;

                    default:
                        Console.WriteLine("geçersiz giriş"); break;
                }
            }
            
        }
    }
}
