using System.Windows;

namespace IvanaDrugi.Views
{
    public partial class TemaProzor : Window
    {
        public string IzabranaTema { get; private set; } = "";

        public TemaProzor()
        {
            InitializeComponent();
        }

        private void BtnSvijetla_Click(object sender, RoutedEventArgs e)
        {
            IzabranaTema = "svijetla"; 
            this.DialogResult = true;
        }

        private void BtnTamna_Click(object sender, RoutedEventArgs e)
        {
            IzabranaTema = "tamna"; 
            this.DialogResult = true;
        }

        private void BtnRoze_Click(object sender, RoutedEventArgs e)
        {
            IzabranaTema = "roze"; 
            this.DialogResult = true;
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}