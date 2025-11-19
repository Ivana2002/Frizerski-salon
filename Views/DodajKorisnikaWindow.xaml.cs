// Views/DodajKorisnikaWindow.xaml.cs
using IvanaDrugi.Core.Models;
using IvanaDrugi.Core.Services;
using System.Windows;
using System.Windows.Controls;

namespace IvanaDrugi.Views
{
    public partial class DodajKorisnikaWindow : Window
    {
        public DodajKorisnikaWindow()
        {
            InitializeComponent();
        }

        private void BtnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrEmpty(txtIme.Text) ||
                string.IsNullOrEmpty(txtPrezime.Text) ||
                string.IsNullOrEmpty(txtKontakt.Text) ||
                string.IsNullOrEmpty(txtKorisnickoIme.Text) ||
                string.IsNullOrEmpty(txtLozinka.Password) ||
                string.IsNullOrEmpty(txtPotvrdaLozinke.Password) ||
                cmbUloga.SelectedItem == null ||
                cmbTema.SelectedItem == null ||
                cmbJezik.SelectedItem == null)
            {
                MessageBox.Show("Sva polja su obavezna!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtLozinka.Password != txtPotvrdaLozinke.Password)
            {
                MessageBox.Show("Lozinke se ne poklapaju!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var novaLozinkaHash = BCrypt.Net.BCrypt.HashPassword(txtLozinka.Password);

            var noviKorisnik = new Korisnik
            {
                KorisnickoIme = txtKorisnickoIme.Text.Trim(),
                LozinkaHash = novaLozinkaHash,
                Uloga = ((ComboBoxItem)cmbUloga.SelectedItem).Content.ToString(),
                Ime = txtIme.Text.Trim(),
                Prezime = txtPrezime.Text.Trim(),
                PreferiranaTema = ((ComboBoxItem)cmbTema.SelectedItem).Content.ToString(),
                JezikKoda = ((ComboBoxItem)cmbJezik.SelectedItem).Content.ToString()
            };

            try
            {
                var dbService = new DatabaseService();
                dbService.DodajKorisnika(noviKorisnik); 
                MessageBox.Show($"Korisnik '{noviKorisnik.Ime} {noviKorisnik.Prezime}' je uspešno dodat!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom dodavanja korisnika: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}