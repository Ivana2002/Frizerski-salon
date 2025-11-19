using IvanaDrugi.Core.Models;
using IvanaDrugi.Core.Services;
using System.Windows;

namespace IvanaDrugi.Views
{
    public partial class UrediKorisnikaWindow : Window
    {
        private readonly Korisnik _korisnik;

        public UrediKorisnikaWindow(Korisnik korisnik)
        {
            InitializeComponent();
            _korisnik = korisnik;
            PopuniPolja();
        }

        private void PopuniPolja()
        {
            txtIme.Text = _korisnik.Ime;
            txtPrezime.Text = _korisnik.Prezime;
            txtKorisnickoIme.Text = _korisnik.KorisnickoIme;
            cmbUloga.Text = _korisnik.Uloga;
            cmbTema.Text = _korisnik.PreferiranaTema;
            cmbJezik.Text = _korisnik.JezikKoda;
        }

        private void BtnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            var db = new DatabaseService();
            _korisnik.Ime = txtIme.Text;
            _korisnik.Prezime = txtPrezime.Text;
            _korisnik.KorisnickoIme = txtKorisnickoIme.Text;
            _korisnik.Uloga = cmbUloga.Text;
            _korisnik.PreferiranaTema = cmbTema.Text;
            _korisnik.JezikKoda = cmbJezik.Text;

            
            if (!string.IsNullOrEmpty(txtLozinka.Password))
            {
                _korisnik.LozinkaHash = txtLozinka.Password; 
            }

            
            db.AzurirajKorisnika(_korisnik);
            this.DialogResult = true;
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}