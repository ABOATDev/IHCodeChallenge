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
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Search;

namespace IHCode
{

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

            //this.codeBox.Background = UsefulColors.DARK_BACKROUND.GetBrush();
            //this.codeBox.Foreground = UsefulColors.BRIGHT_CODE_COLOR.GetBrush();
            this.codeBox.Background = UsefulColors.BRIGHT_CODE_COLOR.GetBrush();
            this.codeBox.Foreground = UsefulColors.DARKER_BACKGROUND.GetBrush();
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

            CodeFile file = GetCurrentCodeFile();

            bool isNewFile = (file is null);

            string filePath = string.Empty;

            if (isNewFile)
            {

                MessageBoxResult result = MessageBox.Show("The file you are trying to save is not associated to any known file, save it to a new file ?", "Unknown file", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {

                    filePath = UserInteractions.GetSaveFileDialogFilePath();

                } else
                {

                    return;

                }

            } else
            {

                filePath = file.FullPath;

            }

            string content = GetCodeBoxContent();

            bool success = fileManager.SaveFile(filePath, content);

            if (success) {

                if (isNewFile)
                {

                    fileManager.AddFile(filePath);
                    UpdateFileList();

                }

                DisplayInformation("File saved.", InformationType.SUCCESS);
                this.Title = this.Title.Replace("*", string.Empty);

            } else {

                DisplayInformation("Could not save file.", InformationType.ERROR);

            }

        }

        public CodeFile GetCurrentCodeFile()
        {

            if (this.fileList.SelectedIndex == -1) { return null; }

            return fileManager.Files.Where(c => c.FriendlyName == fileList.SelectedItem.ToString()).FirstOrDefault();

        }

        private string GetCodeBoxContent()
        {

            TextDocument currentDocument = codeBox.Document;

            return currentDocument.Text;

        }

        private void SetCodeBoxContent(string text)
        {

            TextDocument futureDocument = new TextDocument();

            futureDocument.Text = text;

            this.codeBox.Document = futureDocument;


        }

        private void OpenLoadedFile(object sender, SelectionChangedEventArgs e)
        {

            int selectedIndex = fileList.SelectedIndex;

            if (selectedIndex != -1)
            {

                CodeFile file = GetCurrentCodeFile();
                string path = file.FullPath;
                string text = System.IO.File.ReadAllText(path);

                //Changement de la coloration selon le langage
                codeBox.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(System.IO.Path.GetExtension(path));
                codeBox.ShowLineNumbers = true;

                SetCodeBoxContent(text);

                this.Title = GetCurrentCodeFile().FullPath;
                infoTextFile.Text = fileManager.InfoFile(path);
            }
            
        }

        private void codeBox_TextChanged(object sender, EventArgs e)
        {
            
            int selectedIndex = fileList.SelectedIndex;

            if (selectedIndex != -1)
            {

                CodeFile file = GetCurrentCodeFile();

                if (file is null) { this.infoTextFile.Text = string.Empty; return; }

                string path = file.FullPath;
                string textfile = System.IO.File.ReadAllText(path);

                if (GetCodeBoxContent().ToString() != textfile)
                {
                    if (Title.Substring(Title.Length - 1, 1) != "*")
                    {
                        Title += "*";
                    }
                }
                else
                {
                    if (Title.Substring(Title.Length - 1, 1) == "*")
                    {
                        Title = Title.Replace("*", "");
                    }
                    
                }

            }
            else
            {
                Title = "New file";
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

                e.Handled = true;

                SaveFile(null, null);

            }

            if (e.Key == Key.Up)
            {
                e.Handled = true;
                try {fileList.SelectedIndex -= 1;}
                catch {}

                
            }

            if (e.Key == Key.Down)
            {
                e.Handled = true;
                fileList.SelectedIndex += 1;
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
            fileManager.Files.Remove(GetCurrentCodeFile());
            SetCodeBoxContent(string.Empty);
            UpdateFileList();
            this.Title = "Instant Hack Code";
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {

            string oldFileName = GetCurrentCodeFile().FullPath;
            string directory = System.IO.Path.GetDirectoryName(oldFileName);
            string fileName = new IBox().ShowDialog("Set new file name :");
            string newFileName = directory + System.IO.Path.DirectorySeparatorChar + fileName;

            if (String.IsNullOrEmpty(fileName)) { return; }

            if (!newFileName.EndsWith(".js"))
            {

                newFileName += ".js";

            }

            //Renommé le fichier
            if (fileManager.RenameFile(oldFileName, newFileName)) 
            {

                fileManager.Files.RemoveAt(fileList.SelectedIndex);

                fileManager.AddFile(newFileName);

                DisplayInformation("File renamed.", InformationType.SUCCESS); 

            }
            else
            {
                DisplayInformation("Could not rename file.", InformationType.ERROR);
            }

            UpdateFileList();

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            //Delete le fichier
            if (fileManager.DeleteFile(GetCurrentCodeFile().FullPath))
            {
                //Enlever de la liste
                fileManager.Files.Remove(GetCurrentCodeFile());
                SetCodeBoxContent(string.Empty);
                DisplayInformation("File deleted.", MainWindow.InformationType.SUCCESS);
                UpdateFileList();
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

                if (!fileManager.AddFile(file))
                {

                    DisplayInformation("File " + file + " already exists.", InformationType.WARNING);

                }

            }
            UpdateFileList();
        }

        public enum InformationType
        {
            ERROR,
            INFO,
            WARNING,
            SUCCESS
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFile(sender, e);   //Ou SaveFile(null, null);
        }

    }
}
