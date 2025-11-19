using IvanaDrugi.Core.Services;
using System.Windows;
using System.Windows.Controls;

namespace IvanaDrugi.Views
{
    public partial class ZakazaneRezervacijeView : UserControl
    {
        public ZakazaneRezervacijeView()
        {
            InitializeComponent();
            UcitajZakazane();
        }

        private void UcitajZakazane()
        {
            var db = new DatabaseService();
            dgZakazane.ItemsSource = db.GetZakazaneRezervacije();
        }

        private void BtnZapocni_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int rezId)
            {
                try
                {
                    var db = new DatabaseService();
                    db.ZapocniRezervaciju(rezId);

                    
                    if (Application.Current.MainWindow is MainView mainView)
                    {
                        mainView.MainContent.Content = new TrenutnoUTokuView(mainView.TrenutniKorisnik);
                    }

                    MessageBox.Show("Rezervacija prebačena u 'u toku'!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška: {ex.Message}");
                }
            }
        }
    }
}