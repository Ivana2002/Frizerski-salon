using System;
using System.Windows;
using System.Windows.Controls;

namespace IvanaDrugi.Views
{
    public partial class PregledRezervacijaView : UserControl
    {
        public PregledRezervacijaView()
        {
            InitializeComponent();
            UcitajRezervacije();
        }

        private void UcitajRezervacije()
        {
            var db = new Core.Services.DatabaseService();
            DataGridRezervacije.ItemsSource = db.GetSveRezervacije();
        }

        private void BtnPretraziPoDatumu_Click(object sender, RoutedEventArgs e)
        {
            if (dpDatum.SelectedDate.HasValue)
            {
                var db = new Core.Services.DatabaseService();
                DataGridRezervacije.ItemsSource = db.GetRezervacijePoDatumu(dpDatum.SelectedDate.Value);
            }
            else
            {
                UcitajRezervacije();
            }
        }
    }
}