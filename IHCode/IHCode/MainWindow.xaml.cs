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

        private FileManager fileManager = new FileManager();

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

        private void OpenFolder(object sender, RoutedEventArgs e)
        {

            string selectedDirectory = UserInteractions.GetOpenFolderDialogResult();
            bool succesfullyOpened = fileManager.OpenDirectory(selectedDirectory);

            if (succesfullyOpened)
            {

                fileManager.CurrentDirectory = selectedDirectory;

                UpdateFileList();

                this.DisplayInformation("Succesfully opened workspace files.", InformationType.SUCCESS);

            } else
            {

                this.DisplayInformation("Could not open workspace.", InformationType.ERROR);

            }



        }

        public void UpdateFileList()
        {

            this.fileList.Items.Clear();

            foreach (string filePath in fileManager.Files)
            {

                string cleanName = System.IO.Path.GetFileName(filePath);

                this.fileList.Items.Add(cleanName);

            }

        }

        public void DisplayInformation(string message, InformationType type)
        {

            switch (type)
            {

                case InformationType.ERROR:

                    infoTextBlock.Foreground = Colors.Red.GetBrush();
                    break;

                case InformationType.INFO:

                    infoTextBlock.Foreground = Colors.White.GetBrush();
                    break;

                case InformationType.SUCCESS:

                    infoTextBlock.Foreground = Colors.Green.GetBrush();
                    break;

                case InformationType.WARNING:
                    
                    infoTextBlock.Foreground = Colors.Orange.GetBrush();
                    break;
            }

            infoTextBlock.Text = message;

        }

        public enum InformationType
        {

            ERROR,
            INFO,
            WARNING,
            SUCCESS

        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {

            string filePath = GetCurrentFilePath();
            string content = GetCodeBoxContent();

            bool success = fileManager.SaveFile(filePath, content);

            if (success) {

                DisplayInformation("File saved.", MainWindow.InformationType.SUCCESS);

            } else {

                DisplayInformation("Could not save file.", InformationType.ERROR);

            }

        }

        private string GetCurrentFilePath()
        {

            return fileManager.CurrentDirectory + System.IO.Path.DirectorySeparatorChar + fileList.SelectedItem.ToString();

        }

        private string GetCodeBoxContent()
        {

            FlowDocument currentDocument = codeBox.Document;

            TextPointer start = currentDocument.ContentStart;
            TextPointer end = currentDocument.ContentEnd;

            TextRange range = new TextRange(start, end);

            return range.Text;

        }

        private void SetCodeBoxContent(string text)
        {

            FlowDocument futureDocument = new FlowDocument();

            futureDocument.Blocks.Add(new Paragraph(new Run(text)));

            this.codeBox.Document = futureDocument;

        }

        private void OpenLoadedFile(object sender, SelectionChangedEventArgs e)
        {

            string path = GetCurrentFilePath();
            string text = System.IO.File.ReadAllText(path);

            SetCodeBoxContent(text);

        }

    }
}
