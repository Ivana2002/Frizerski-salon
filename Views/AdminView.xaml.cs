// Views/AdminView.xaml.cs
using IvanaDrugi.Core.Models;
using IvanaDrugi.Core.Services;
using System.Windows;

namespace IvanaDrugi.Views
{
    public partial class AdminView : Window
    {
        public AdminView()
        {
            InitializeComponent();
            PostaviUIPoUlozi();
        }

        private void PostaviUIPoUlozi()
        {
            var korisnik = Application.Current.Properties["TrenutniKorisnik"] as Korisnik;

            if (korisnik != null)
            {
                this.Title = $"Frizerski Salon - {korisnik.Uloga}: {korisnik.Ime} {korisnik.Prezime}";
               
            }
        }

        
        private void BtnPregledUsluga_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UslugeView();
        }

        private void BtnDodajUslugu_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new DodajUsluguWindow();
        }

        private void BtnPretragaUsluga_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UslugeView(); 
        }

        private void BtnObrisiUslugu_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UslugeView(); 
        }

        private void BtnKreirajRezervaciju_Click(object sender, RoutedEventArgs e)
        {
            var trenutniKorisnik = Application.Current.Properties["TrenutniKorisnik"] as Korisnik;
            if (trenutniKorisnik == null)
            {
                MessageBox.Show("Korisnik nije prijavljen.");
                return;
            }
            MainContent.Content = new KreirajRezervacijuView(trenutniKorisnik);
        }

        private void BtnPregledRezervacija_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new PregledRezervacijaView();
        }

        private void BtnKreirajRacun_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new RacuniView();
        }

        private void BtnPregledRacuna_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new RacuniView();
        }

        private void BtnPretragaRacuna_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new RacuniView(); 
        }

        private void BtnUpravljanjeKorisnicima_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UpravljanjeKorisnicimaView();
        }

        private void BtnPromenaTeme_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Promena teme je implementirana u postavkama.");
        }

        private void BtnPromenaJezika_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Promena jezika je implementirana u postavkama.");
        }

        
        private void BtnDodajKorisnika_Click(object sender, RoutedEventArgs e)
        {
            var korisnik = Application.Current.Properties["TrenutniKorisnik"] as Korisnik;

            if (korisnik?.Uloga != "admin")
            {
                MessageBox.Show("Samo administrator može da doda novog korisnika!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dodajProzor = new DodajKorisnikaWindow();
            if (dodajProzor.ShowDialog() == true)
            {
                MessageBox.Show("Korisnik je uspešno dodat!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}