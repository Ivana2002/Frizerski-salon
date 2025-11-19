using IvanaDrugi.Core.Models;
using IvanaDrugi.Views;
using System.Windows;

namespace IvanaDrugi
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

           
            ApplyTheme("svijetla");

            var login = new LoginView();
            bool? rezultat = login.ShowDialog();

            if (rezultat == true)
            {
                var korisnik = Application.Current.Properties["TrenutniKorisnik"] as Korisnik;

                if (korisnik != null)
                {
                    string unesiTema = korisnik.PreferiranaTema?.Trim().ToLower() ?? "svijetla";
                    string tema = unesiTema switch
                    {
                        "svijetla" => "svijetla",   
                        "tamna" => "tamna",         
                        "roze" => "roze",           
                        _ => "svijetla"
                    };
                    ApplyTheme(tema);

                    var mainView = new MainView(korisnik);
                    this.MainWindow = mainView;
                    mainView.Show();
                }
                else
                {
                    MessageBox.Show("Korisnik nije pronađen.");
                    Shutdown();
                }
            }
            else
            {
                Shutdown();
            }
        }

        public static void ApplyTheme(string themeName)
        {
            var themePath = $"Themes/{themeName}.xaml"; 
            var dict = new ResourceDictionary { Source = new Uri(themePath, UriKind.Relative) };

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}