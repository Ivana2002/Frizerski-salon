using IvanaDrugi.Core.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace IvanaDrugi.Views
{
    public partial class RacuniSearchView : UserControl
    {
        public RacuniSearchView()
        {
            InitializeComponent();
        }

        private void BtnPretrazi_Click(object sender, RoutedEventArgs e)
        {
            if (dpDatum.SelectedDate.HasValue)
            {
                var db = new DatabaseService();
                DataGridRacuni.ItemsSource = db.GetRacunePoDatumu(dpDatum.SelectedDate.Value);
            }
            else
            {
                DataGridRacuni.ItemsSource = null;
            }
        }
    }
}