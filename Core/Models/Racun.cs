// Core/Models/Racun.cs
namespace IvanaDrugi.Core.Models
{
    public class Racun
    {
        public int RacunId { get; set; }
        public string BrojRacuna { get; set; } = string.Empty;
        public DateTime DatumIzdavanja { get; set; } = DateTime.Now;
        public decimal UkupnaCijena { get; set; } = 0;

        public int KlijentId { get; set; }
        public Klijent Klijent { get; set; } = null!;

        public int RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; } = null!;

        public List<StavkaRacuna> StavkeRacuna { get; set; } = new();

        public int IzdaoKorisnikId { get; set; }
    }
}