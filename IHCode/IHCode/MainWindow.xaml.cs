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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IHCode
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();

            InitializeColors();

        }

        public void InitializeColors()
        {

            this.codeBox.Background = UsefulColors.DARK_BACKROUND.GetBrush();
            this.codeBox.Foreground = UsefulColors.BRIGHT_CODE_COLOR.GetBrush();

            this.Background         = UsefulColors.DARKER_BACKGROUND.GetBrush();

            this.openButton.Background = UsefulColors.FILE_BUTTONS_COLOR.GetBrush();
            this.saveButton.Background = UsefulColors.FILE_BUTTONS_COLOR.GetBrush();

            this.fileList.Background = UsefulColors.FILE_BUTTONS_COLOR.GetBrush();

        }

    }
}
