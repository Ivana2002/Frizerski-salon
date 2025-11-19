using IvanaDrugi.Resources;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace IvanaDrugi.Views
{
    public partial class UslugeView : UserControl
    {
        public UslugeView()
        {
            InitializeComponent();
            UcitajLokalizovaneTekstove();
            try
            {
                UcitajUsluge();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }

         
            if (Application.Current.Properties["TrenutniKorisnik"] is Core.Models.Korisnik korisnik && korisnik.Uloga == "radnik")
            {
                btnDodajUslugu.Visibility = Visibility.Collapsed;
                DataGridUsluge.Columns[5].Visibility = Visibility.Collapsed; 
            }
        }

        private void UcitajLokalizovaneTekstove()
        {
            var loc = LocalizationManager.Instance;
            txtNaslov.Text = loc.GetString("Services_Title");
            btnDodajUslugu.Content = loc.GetString("Services_Add");
            txtPretragaLabel.Text = loc.GetString("Services_SearchByTitle");
        }

        private void UcitajUsluge()
        {
            var dbService = new Core.Services.DatabaseService();
            var usluge = dbService.GetSveUsluge();
            DataGridUsluge.ItemsSource = usluge;
        }

        private void TxtPretraga_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var dbService = new Core.Services.DatabaseService();
            var sveUsluge = dbService.GetSveUsluge();

            if (!string.IsNullOrEmpty(txtPretraga.Text))
            {
                var filtrirane = sveUsluge.Where(u => u.Naziv.ToLower().Contains(txtPretraga.Text.ToLower())).ToList();
                DataGridUsluge.ItemsSource = filtrirane;
            }
            else
            {
                DataGridUsluge.ItemsSource = sveUsluge;
            }
        }

        private void BtnDodajUslugu_Click(object sender, RoutedEventArgs e)
        {
            var dodajProzor = new DodajUsluguWindow();
            if (dodajProzor.ShowDialog() == true)
            {
                UcitajUsluge();
            }
        }

        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            var dugme = sender as Button;
            var usluga = dugme?.DataContext as Core.Models.Usluga;

            if (usluga == null) return;

            var result = MessageBox.Show(
                $"Da li ste sigurni da želite da obrišete uslugu '{usluga.Naziv}'?",
                "Potvrda brisanja",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                try
                {
                    var dbService = new Core.Services.DatabaseService();
                    dbService.ObrisiUslugu(usluga.UslugaId);
                    MessageBox.Show($"Usluga '{usluga.Naziv}' je obrisana!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                    UcitajUsluge();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Greška: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}