using System.Windows;

namespace IvanaDrugi.Views
{
    public partial class JezikProzor : Window
    {
        public string IzabraniJezik { get; private set; } = "";

        public JezikProzor()
        {
            InitializeComponent();
        }

        private void BtnSrpski_Click(object sender, RoutedEventArgs e)
        {
            IzabraniJezik = "sr";
            this.DialogResult = true;
        }

        private void BtnEnglish_Click(object sender, RoutedEventArgs e)
        {
            IzabraniJezik = "en";
            this.DialogResult = true;
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}