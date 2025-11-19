using IvanaDrugi.Resources;
using System;
using System.Windows;
using System.Windows.Controls;

namespace IvanaDrugi.Views
{
    public partial class RacuniView : UserControl
    {
        public RacuniView()
        {
            InitializeComponent();
            txtNaslov.Text = "Računi frizerskog salona";
            UcitajRacune();
        }

        private void UcitajLokalizovaneTekstove()
        {
            var loc = LocalizationManager.Instance;
            txtNaslov.Text = loc.GetString("Invoices_Title");
        }

        private void UcitajRacune()
        {
            var db = new Core.Services.DatabaseService();
            DataGridRacuni.ItemsSource = db.GetSveRacune();
        }

       
    }
}