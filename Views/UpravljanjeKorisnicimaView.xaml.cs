using IvanaDrugi.Core.Models;
using IvanaDrugi.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace IvanaDrugi.Views
{
    public partial class UpravljanjeKorisnicimaView : UserControl
    {
        private ObservableCollection<Korisnik> _korisnici;

        public UpravljanjeKorisnicimaView()
        {
            InitializeComponent();
            UcitajKorisnike();
        }

        private void UcitajKorisnike()
        {
            var db = new DatabaseService();
            _korisnici = new ObservableCollection<Korisnik>(db.GetSveKorisnike());
            dgKorisnici.ItemsSource = _korisnici;
        }

        private void BtnDodajKorisnika_Click(object sender, RoutedEventArgs e)
        {
            var dodaj = new DodajKorisnikaWindow();
            if (dodaj.ShowDialog() == true)
            {
                UcitajKorisnike();
            }
        }

        private void BtnUredi_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int korisnikId)
            {
                var korisnik = _korisnici.FirstOrDefault(k => k.KorisnikId == korisnikId);
                if (korisnik != null)
                {
                    var uredi = new UrediKorisnikaWindow(korisnik);
                    if (uredi.ShowDialog() == true)
                    {
                        UcitajKorisnike();
                    }
                }
            }
        }

        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int korisnikId)
            {
                var result = MessageBox.Show(
                    "Da li ste sigurni da želite da obrišete korisnika?",
                    "Potvrda brisanja",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    var db = new DatabaseService();
                    db.ObrisiKorisnika(korisnikId);
                    UcitajKorisnike();
                }
            }
        }
    }
}