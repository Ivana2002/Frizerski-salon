// Views/DodajUsluguWindow.xaml.cs
using IvanaDrugi.Core.Models;
using IvanaDrugi.Core.Services;
using System.Windows;

namespace IvanaDrugi.Views
{
    public partial class DodajUsluguWindow : Window
    {
        public DodajUsluguWindow()
        {
            InitializeComponent();
        }

        private void BtnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            // Validacija
            if (string.IsNullOrEmpty(txtSifra.Text) ||
                string.IsNullOrEmpty(txtNaziv.Text) ||
                string.IsNullOrEmpty(txtCena.Text) ||
                string.IsNullOrEmpty(txtTrajanje.Text))
            {
                MessageBox.Show("Sva polja su obavezna!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtCena.Text, out decimal cena) || cena <= 0)
            {
                MessageBox.Show("Cijena mora biti pozitivan broj!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtTrajanje.Text, out int trajanje) || trajanje <= 0)
            {
                MessageBox.Show("Trajanje mora biti pozitivan broj!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var novaUsluga = new Usluga
            {
                Sifra = txtSifra.Text.Trim(),
                Naziv = txtNaziv.Text.Trim(),
                Opis = txtOpis.Text.Trim(),
                Cena = cena,
                TrajanjeMinuta = trajanje,
                Aktivan = true,
                KategorijaId = 1 // Privremeno - kasnije možeš dodati ComboBox za kategoriju
            };

            try
            {
                var dbService = new DatabaseService();
                dbService.DodajUslugu(novaUsluga); // ← OVO DODAJE U BAZU!
                MessageBox.Show($"Usluga '{novaUsluga.Naziv}' je dodata u bazu!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true; // Zatvori prozor i vrati true
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom dodavanja usluge: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Zatvori prozor bez dodavanja
        }
    }
}