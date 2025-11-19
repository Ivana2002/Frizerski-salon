using IvanaDrugi.Core.Models;
using IvanaDrugi.Core.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace IvanaDrugi.Views
{
    public partial class TrenutnoUTokuView : UserControl
    {
        private readonly int _trenutniKorisnikId;

        public TrenutnoUTokuView(Korisnik trenutniKorisnik)
        {
            InitializeComponent();
            _trenutniKorisnikId = trenutniKorisnik.KorisnikId;
            UcitajAktivne();
        }

        private void UcitajAktivne()
        {
            var db = new DatabaseService();
            var rezervacije = db.GetAktivneRezervacije();
            dgAktivne.ItemsSource = rezervacije;

            
            txtPrazno.Visibility = rezervacije.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnZavrsi_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int rezId)
            {
                try
                {
                    var db = new DatabaseService();

                    
                    var sveRez = db.GetSveRezervacije();
                    var rezervacija = sveRez.Find(r => r.RezervacijaId == rezId);
                    if (rezervacija == null)
                    {
                        MessageBox.Show("Rezervacija nije pronađena.");
                        return;
                    }

                    
                    var usluge = db.GetUslugeZaRezervaciju(rezId);
                    decimal ukupno = 0;
                    foreach (var u in usluge)
                        ukupno += u.Cena;

                    
                    var racun = new Racun
                    {
                        BrojRacuna = $"R-{DateTime.Now:yyyyMMddHHmmss}",
                        DatumIzdavanja = DateTime.Now,
                        UkupnaCijena = ukupno,
                        KlijentId = rezervacija.KlijentId,
                        RezervacijaId = rezId,
                        IzdaoKorisnikId = _trenutniKorisnikId 
                    };

                    db.IzdajRacun(racun);

                    
                    db.ZavrsiRezervaciju(rezId);

                    MessageBox.Show("Račun izdat i usluga završena!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);

                    
                    UcitajAktivne();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}