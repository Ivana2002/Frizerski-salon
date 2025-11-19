using IvanaDrugi.Core.Models;
using IvanaDrugi.Core.Services;
using System.Windows;
using System.Windows.Controls;

namespace IvanaDrugi.Views
{
    public partial class DodajUsluguView : UserControl
    {
        public DodajUsluguView()
        {
            InitializeComponent();
        }

        private void BtnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
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
                KategorijaId = 1
            };

            try
            {
                var dbService = new DatabaseService();
                dbService.DodajUslugu(novaUsluga);
                MessageBox.Show($"Usluga '{novaUsluga.Naziv}' je dodata u bazu!", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);

                
                txtSifra.Clear();
                txtNaziv.Clear();
                txtOpis.Clear();
                txtCena.Clear();
                txtTrajanje.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom dodavanja usluge: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            
            txtSifra.Clear();
            txtNaziv.Clear();
            txtOpis.Clear();
            txtCena.Clear();
            txtTrajanje.Clear();
        }
    }
}
