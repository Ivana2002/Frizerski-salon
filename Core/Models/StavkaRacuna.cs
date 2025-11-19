// Core/Models/StavkaRacuna.cs
namespace IvanaDrugi.Core.Models
{
    public class StavkaRacuna
    {
        public int StavkaId { get; set; }
        public int RacunId { get; set; }
        public int UslugaId { get; set; }
        public string NazivUsluge { get; set; } = string.Empty;
        public decimal CenaUsluge { get; set; } = 0;
        public int Kolicina { get; set; } = 1;

        
        public Racun? Racun { get; set; }
        public Usluga? Usluga { get; set; }
    }
}