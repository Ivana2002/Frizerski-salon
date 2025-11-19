using IvanaDrugi.Core.Models;
using IvanaDrugi.Core.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace IvanaDrugi.Views
{
    public partial class KreirajRacunView : UserControl
    {
        private readonly Korisnik _trenutniKorisnik;

        public KreirajRacunView(Korisnik trenutniKorisnik)
        {
            InitializeComponent();
            _trenutniKorisnik = trenutniKorisnik;
            UcitajKlijente();
            dpDatum.SelectedDate = DateTime.Now;
        }

        private void UcitajKlijente()
        {
            var db = new DatabaseService();
            cmbKlijenti.ItemsSource = db.GetSveKlijente();
        }

        private void BtnIzdaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBrojRacuna.Text))
            {
                MessageBox.Show("Unesite broj računa.");
                return;
            }

            if (cmbKlijenti.SelectedItem == null)
            {
                MessageBox.Show("Odaberite klijenta.");
                return;
            }

            if (!decimal.TryParse(txtCijena.Text, out decimal cijena) || cijena <= 0)
            {
                MessageBox.Show("Unesite ispravnu cijenu.");
                return;
            }

            var racun = new Racun
            {
                BrojRacuna = txtBrojRacuna.Text,
                DatumIzdavanja = dpDatum.SelectedDate ?? DateTime.Now,
                UkupnaCijena = cijena,
                KlijentId = ((Klijent)cmbKlijenti.SelectedItem).KlijentId, 
                RezervacijaId = 0,
                IzdaoKorisnikId = _trenutniKorisnik.KorisnikId
            };

            var db = new DatabaseService();
            db.IzdajRacun(racun);
            MessageBox.Show("Račun uspješno izdat!", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}