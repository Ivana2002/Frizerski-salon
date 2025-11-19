using IvanaDrugi.Core.Models;
using IvanaDrugi.Core.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace IvanaDrugi.Views
{
    public partial class KreirajRezervacijuView : UserControl
    {
        private readonly Korisnik _trenutniKorisnik;

        
        public KreirajRezervacijuView(Korisnik trenutniKorisnik)
        {
            InitializeComponent();
            _trenutniKorisnik = trenutniKorisnik;
            UcitajUsluge();
            UcitajSate();
        }

        
        public KreirajRezervacijuView()
        {
            InitializeComponent();
            UcitajUsluge();
            UcitajSate();
        }

        private void UcitajUsluge()
        {
            var db = new DatabaseService();
            cmbUsluge.ItemsSource = db.GetSveUsluge();
            cmbUsluge.DisplayMemberPath = "Naziv";
        }

        private void UcitajSate()
        {
            for (int i = 8; i <= 20; i++)
            {
                cmbSat.Items.Add($"{i:D2}:00");
            }
        }

        private void BtnKreiraj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtImeKlijenta.Text) || cmbUsluge.SelectedItem == null || !dpDatum.SelectedDate.HasValue)
            {
                MessageBox.Show("Molimo unesite sve podatke.");
                return;
            }

            try
            {
                var db = new DatabaseService();

                
                var klijent = new Klijent
                {
                    PunoIme = txtImeKlijenta.Text,
                    Telefon = txtTelefon.Text
                };
                int klijentId = db.DodajKlijenta(klijent);

                
                var sat = TimeSpan.Parse(cmbSat.SelectedItem.ToString());
                var datumVreme = dpDatum.SelectedDate.Value + sat;

                var rezervacija = new Rezervacija
                {
                    KlijentId = klijentId,
                    DodeljenKorisnikId = _trenutniKorisnik?.KorisnikId ?? 1,
                    DatumVreme = datumVreme,
                    Status = "zakazano"
                };
                int rezervacijaId = db.DodajRezervaciju(rezervacija);

                
                var usluga = (Usluga)cmbUsluge.SelectedItem;
                db.DodajUsluguZaRezervaciju(rezervacijaId, usluga.UslugaId);

                MessageBox.Show("Rezervacija uspješno kreirana!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }
    }
}