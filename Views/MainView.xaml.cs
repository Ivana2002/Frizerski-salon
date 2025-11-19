using IvanaDrugi.Core.Models;
using IvanaDrugi.Resources;
using System.Windows;
using System.Windows.Controls;

namespace IvanaDrugi.Views
{
    public partial class MainView : Window
    {
        private readonly Korisnik _korisnik;
        public Korisnik TrenutniKorisnik => _korisnik;

        public MainView(Korisnik korisnik)
        {
            InitializeComponent();
            _korisnik = korisnik;

            UcitajLokalizovaneTekstove();
            PostaviUI();
        }

        private void UcitajLokalizovaneTekstove()
        {
            var loc = LocalizationManager.Instance;

            
            menuUsluge.Header = loc.GetString("MainMenu_Services");
            menuRezervacije.Header = loc.GetString("MainMenu_Reservations");
            menuRacuni.Header = loc.GetString("MainMenu_Invoices");
            menuPostavke.Header = loc.GetString("MainMenu_Settings");

           
            var uslugeItems = menuUsluge.Items;
            if (uslugeItems.Count >= 4)
            {
                ((MenuItem)uslugeItems[0]).Header = loc.GetString("Services_View");
                ((MenuItem)uslugeItems[1]).Header = loc.GetString("Services_Add");
                ((MenuItem)uslugeItems[2]).Header = loc.GetString("Services_Search");
                ((MenuItem)uslugeItems[3]).Header = loc.GetString("Services_Delete");
            }

            
            var rezervacijeItems = menuRezervacije.Items;
            if (rezervacijeItems.Count >= 2)
            {
                ((MenuItem)rezervacijeItems[0]).Header = loc.GetString("Reservations_Create");
                ((MenuItem)rezervacijeItems[1]).Header = loc.GetString("Reservations_View");
            }

            
            var racuniItems = menuRacuni.Items;
            if (racuniItems.Count >= 3)
            {
                ((MenuItem)racuniItems[0]).Header = loc.GetString("Invoices_Create");
                ((MenuItem)racuniItems[1]).Header = loc.GetString("Invoices_View");
                ((MenuItem)racuniItems[2]).Header = loc.GetString("Invoices_SearchByDate");
            }

           
            var postavkeItems = menuPostavke.Items;
            if (postavkeItems.Count >= 3)
            {
                ((MenuItem)postavkeItems[0]).Header = loc.GetString("Settings_ManageUsers");
                ((MenuItem)postavkeItems[1]).Header = loc.GetString("Settings_ChangeTheme");
                ((MenuItem)postavkeItems[2]).Header = loc.GetString("Settings_ChangeLanguage");
            }

            btnDodajKorisnika.Content = loc.GetString("Button_AddUser");
        }

        private void PostaviUI()
        {
            
            if (_korisnik.Uloga?.ToLower() != "admin")
            {
                btnDodajKorisnika.Visibility = Visibility.Collapsed;

                var postavkeItems = menuPostavke.Items;
                if (postavkeItems.Count > 0)
                {
                    ((MenuItem)postavkeItems[0]).Visibility = Visibility.Collapsed;
                }
            }

           
            for (int i = 1; i < menuUsluge.Items.Count; i++)
            {
                if (menuUsluge.Items[i] is MenuItem item)
                {
                    item.Visibility = Visibility.Collapsed;
                }
            }

            try
            {
                MainContent.Content = new TrenutnoUTokuView(_korisnik);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnPregledUsluga_Click(object sender, RoutedEventArgs e) => MainContent.Content = new UslugeView();
        private void BtnDodajUslugu_Click(object sender, RoutedEventArgs e) => MainContent.Content = new DodajUsluguWindow();
       
        private void BtnObrisiUslugu_Click(object sender, RoutedEventArgs e) => MainContent.Content = new UslugeView();

        private void BtnKreirajRezervaciju_Click(object sender, RoutedEventArgs e)
       => MainContent.Content = new KreirajRezervacijuView(_korisnik);

        private void BtnPregledRezervacija_Click(object sender, RoutedEventArgs e)
            => MainContent.Content = new PregledRezervacijaView();
      
        private void BtnPregledRacuna_Click(object sender, RoutedEventArgs e) => MainContent.Content = new RacuniView();
        private void BtnPretragaRacuna_Click(object sender, RoutedEventArgs e) => MainContent.Content = new RacuniSearchView();

        private void BtnUpravljanjeKorisnicima_Click(object sender, RoutedEventArgs e) => MainContent.Content = new UpravljanjeKorisnicimaView();

        private void BtnPromjenaTeme_Click(object sender, RoutedEventArgs e)
        {
            var prozor = new TemaProzor();
            if (prozor.ShowDialog() == true)
            {
                var tema = prozor.IzabranaTema; 
                var dict = new ResourceDictionary { Source = new System.Uri($"Themes/{tema}.xaml", System.UriKind.Relative) };
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dict);

                var db = new Core.Services.DatabaseService();
                db.SacuvajPreferencu(_korisnik.KorisnikId, tema, _korisnik.JezikKoda);
            }
        }

        private void BtnPromjenaJezika_Click(object sender, RoutedEventArgs e)
        {
            var prozor = new JezikProzor();
            if (prozor.ShowDialog() == true)
            {
                var jezik = prozor.IzabraniJezik;
                LocalizationManager.CurrentLanguage = jezik;

                
                UcitajLokalizovaneTekstove();
                PostaviUI();

                
                var db = new Core.Services.DatabaseService();
                db.SacuvajPreferencu(_korisnik.KorisnikId, _korisnik.PreferiranaTema, jezik);
            }
        }

        private void BtnDodajKorisnika_Click(object sender, RoutedEventArgs e)
        {
            if (_korisnik.Uloga?.ToLower() != "admin")
            {
                MessageBox.Show("Samo administrator može da doda korisnika!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dodaj = new DodajKorisnikaWindow();
            if (dodaj.ShowDialog() == true)
                MessageBox.Show("Korisnik uspješno dodat!", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnZakazane_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ZakazaneRezervacijeView();
        }
    }
}