namespace IvanaDrugi.Core.Models
{
    public class Usluga
    {
        public int UslugaId { get; set; }
        public string Sifra { get; set; } = string.Empty;
        public string Naziv { get; set; } = string.Empty;
        public string? Opis { get; set; }
        public decimal Cena { get; set; }
        public int TrajanjeMinuta { get; set; }
        public bool Aktivan { get; set; } = true;
        public int KategorijaId { get; set; }

        
        public KategorijaUsluge? Kategorija { get; set; }
    }
}