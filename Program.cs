using System;
using System.IO;
using System.Text.Json;

public class CovidConfig
{
    public string satuan_suhu { get; set; }
    public int batas_hari_deman { get; set; }
    public string pesan_ditolak { get; set; }
    public string pesan_diterima { get; set; }

    private const string filePath = "covid_config.json";

    public CovidConfig()
    {

    }

    public void LoadConfig()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            CovidConfig data = JsonSerializer.Deserialize<CovidConfig>(json);

            satuan_suhu = data.satuan_suhu;
            batas_hari_deman = data.batas_hari_deman;
            pesan_ditolak = data.pesan_ditolak;
            pesan_diterima = data.pesan_diterima;
        }
        else
        {
            // default
            satuan_suhu = "celcius";
            batas_hari_deman = 14;
            pesan_ditolak = "Anda tidak diperbolehkan masuk ke dalam gedung ini";
            pesan_diterima = "Anda dipersilahkan untuk masuk ke dalam gedung ini";
            SaveConfig();
        }
    }

    public void SaveConfig()
    {
        string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public void UbahSatuan()
    {
        if (satuan_suhu == "celcius")
        {
            satuan_suhu = "fahrenheit";
        }
        else
        {
            satuan_suhu = "celcius";
        }

        SaveConfig();
    }
}


class Program
{
    static void Main(string[] args)
    {
        CovidConfig config = new CovidConfig();
        config.LoadConfig();

        Console.WriteLine("=== Program Cek Kondisi ===");

        Console.Write("Berapa suhu badan anda saat ini? Dalam nilai " + config.satuan_suhu + ": ");
        double suhu = Convert.ToDouble(Console.ReadLine());

        Console.Write("Berapa hari yang lalu anda terakhir demam? ");
        int hari = Convert.ToInt32(Console.ReadLine());

        bool suhuValid = false;

        if (config.satuan_suhu == "celcius")
        {
            suhuValid = (suhu >= 36.5 && suhu <= 37.5);
        }
        else
        {
            suhuValid = (suhu >= 97.7 && suhu <= 99.5);
        }

        if (suhuValid && hari < config.batas_hari_deman)
        {
            Console.WriteLine(config.pesan_diterima);
        }
        else
        {
            Console.WriteLine(config.pesan_ditolak);
        }

        // Ubah satuan
        Console.WriteLine("\nMengubah satuan suhu...");
        config.UbahSatuan();
        Console.WriteLine("Satuan sekarang: " + config.satuan_suhu);
    }
}