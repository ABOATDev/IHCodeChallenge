using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        private SavedState SaveState = SavedState.Unsaved;


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

            foreach (CodeFile codeFile in fileManager.Files)
            {

                this.fileList.Items.Add(codeFile.FriendlyName);

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

            ClearInfoTextAsync(2000);

        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {

            string filePath = GetCurrentCodeFile().FullPath;
            string content = GetCodeBoxContent();

            bool success = fileManager.SaveFile(filePath, content);

            if (success) {

                DisplayInformation("File saved.", InformationType.SUCCESS);

                SetFileSavedState(SavedState.Saved);

            } else {

                DisplayInformation("Could not save file.", InformationType.ERROR);

            }

        }

        public CodeFile GetCurrentCodeFile()
        {

            return fileManager.Files.Where(c => c.FriendlyName == this.fileList.SelectedItem.ToString()).FirstOrDefault();

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

            if (fileList.SelectedIndex != -1)
            {
                string path = GetCurrentCodeFile().FullPath;
                string text = System.IO.File.ReadAllText(path);

                SetCodeBoxContent(text);

            }
            

        }

        private void ClearInfoTextAsync(int delay)
        {

            Task.Run(() => {

                Thread.Sleep(delay);

                this.infoTextBlock.Dispatcher.Invoke(() => this.infoTextBlock.Text = string.Empty);

            });

        }

        private void PotentialSaveEvent(object sender, KeyEventArgs e)
        {

            if (!Keyboard.IsKeyDown(Key.LeftCtrl))
            {

                return;

            }

            if (e.Key == Key.S)
            {

                SaveFile(null, null);

            }

        }

        private void OnOpOnOpened(object sender, RoutedEventArgs e)
        {
            if (fileList.SelectedIndex == -1)
            {
                cm.IsOpen = false;
            }
            else
            {
                cm.IsOpen = true;
            }
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            //Enlever de la liste
            fileList.Items.Remove(fileList.SelectedItem);
            SetCodeBoxContent(string.Empty);
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            string oldfilename = System.IO.Path.GetFileName(GetCurrentCodeFile().FullPath);
            string path = System.IO.Path.GetDirectoryName(GetCurrentCodeFile().FullPath)+ @"\";
            string newfilename = new InputBox("rename file : (+ extension)").ShowDialog();

                //Renommé le fichier
                if (fileManager.RenameFile(path, oldfilename, newfilename)) 
                {
                fileList.Items[fileList.SelectedIndex] = newfilename;
                DisplayInformation("File renamed.", MainWindow.InformationType.SUCCESS); 
                }
                else
                {
                DisplayInformation("Could not rename file.", InformationType.ERROR);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            //Delete le fichier
            if (fileManager.DeleteFile(GetCurrentCodeFile().FullPath))
            {
                //Enlever de la liste
                fileList.Items.Remove(fileList.SelectedItem);
                SetCodeBoxContent("");
                DisplayInformation("File renamed !.", MainWindow.InformationType.SUCCESS);
            }
            else
            {
                DisplayInformation("Could not delete file.", InformationType.ERROR);
            }


        }

        private void fileList_Drop(object sender, DragEventArgs e)
        {
           
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
            {
                fileList.Items.Add(System.IO.Path.GetFileName(file));
                fileManager.AddFile(file);
            }

        }

        private void SetFileSavedState(SavedState state)
        {

            this.SaveState = state;

            string title = GetCurrentCodeFile().FullPath;

            if (state == SavedState.Unsaved)
            {

                title += "*";

            }

            this.Title = title;

        }

        private void CodeModified(object sender, TextChangedEventArgs e)
        {

            if (SaveState != SavedState.Unsaved)
            {

                SetFileSavedState(SavedState.Unsaved);

            }

        }

        public enum SavedState
        {

            Saved,
            Unsaved

        }

        public enum InformationType
        {

            ERROR,
            INFO,
            WARNING,
            SUCCESS

        }

    }
}
