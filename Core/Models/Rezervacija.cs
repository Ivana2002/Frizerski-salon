// Core/Models/Rezervacija.cs
namespace IvanaDrugi.Core.Models
{
    public class Rezervacija
    {
        public int RezervacijaId { get; set; }
        public int KlijentId { get; set; }
        public int DodeljenKorisnikId { get; set; }
        public DateTime DatumVreme { get; set; }
        public string Status { get; set; } = "zakazano"; 
        public DateTime Kreirano { get; set; } = DateTime.Now;

        
        public Klijent? Klijent { get; set; }
        public Korisnik? DodeljenKorisnik { get; set; }
        public List<Usluga> Usluge { get; set; } = new();
    }
}