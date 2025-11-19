using IvanaDrugi.Resources;
using System.Windows;

namespace IvanaDrugi.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            UcitajTekstove();
        }

        private void UcitajTekstove()
        {
            var loc = LocalizationManager.Instance;
            this.Title = loc.GetString("LoginTitle");
            txtSalonName.Text = loc.GetString("SalonName");
            txtUsernameLabel.Text = loc.GetString("UsernameLabel");
            txtPasswordLabel.Text = loc.GetString("PasswordLabel");
            btnLogin.Content = loc.GetString("LoginButton");
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Unesite korisničko ime i lozinku!", "Greška",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dbService = new Core.Services.DatabaseService();
            var korisnik = dbService.Prijava(username, password);

            if (korisnik != null)
            {
                
                LocalizationManager.CurrentLanguage = korisnik.JezikKoda ?? "sr";
              

                Application.Current.Properties["TrenutniKorisnik"] = korisnik;
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Pogrešno korisničko ime ili lozinka!", "Greška",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}