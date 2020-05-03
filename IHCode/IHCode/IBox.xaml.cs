using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IHCode
{
    /// <summary>
    /// Logique d'interaction pour IBox.xaml
    /// </summary>
    public partial class IBox : Window
    {
        public IBox()
        {
            InitializeComponent();
        }

        public string ShowDialog(string text)
        {

            this.outputBox.Text = text;

            this.ShowDialog();

            return this.inputBox.Text;

        }

        private void ExitDialog(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
