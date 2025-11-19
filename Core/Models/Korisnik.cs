// Core/Models/Korisnik.cs
namespace IvanaDrugi.Core.Models
{
    public class Korisnik
    {
        public int KorisnikId { get; set; }
        public string KorisnickoIme { get; set; } = string.Empty;
        public string LozinkaHash { get; set; } = string.Empty; 
        public string Uloga { get; set; } = "radnik"; 
        public string PreferiranaTema { get; set; } = "svetla";
        public string JezikKoda { get; set; } = "sr";
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
    }
}