// Views/RadnikView.xaml.cs
using IvanaDrugi.Core.Models;
using IvanaDrugi.Core.Services;
using System.Windows;

namespace IvanaDrugi.Views
{
    public partial class RadnikView : Window
    {
        public RadnikView()
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
    }
}