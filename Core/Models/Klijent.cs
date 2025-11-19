// Core/Models/Klijent.cs
namespace IvanaDrugi.Core.Models
{
    public class Klijent
    {
        public int KlijentId { get; set; }
        public string PunoIme { get; set; } = string.Empty;
        public string? Telefon { get; set; }
        public string? Email { get; set; }
    }
}