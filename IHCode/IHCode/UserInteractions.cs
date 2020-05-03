using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IHCode
{
    public static class UserInteractions
    {

        public static string GetOpenFolderDialogResult()
        {

            using (FolderBrowserDialog dialog = GetNewParametizedDialog())
            {

                bool success = (dialog.ShowDialog() == DialogResult.OK);

                if (success)
                {

                    return dialog.SelectedPath;

                } else
                {

                    return string.Empty;

                }

            }

        }

        public static string GetSaveFileDialogFilePath()
        {

            SaveFileDialog saveDialog = GetNewSaveFileDialog();

            bool save = saveDialog.ShowDialog() == DialogResult.OK; 

            if (save)
            {

                return saveDialog.FileName;

            } else
            {

                return string.Empty;

            }

        }

        private static FolderBrowserDialog GetNewParametizedDialog()
        {

            FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.ShowNewFolderButton = true;
            dialog.RootFolder = Environment.SpecialFolder.DesktopDirectory;
            dialog.Description = "Open workspace.";

            return dialog;

        }

        private static SaveFileDialog GetNewSaveFileDialog()
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "JS File|*.js";
            saveFileDialog.Title = "Save file";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            return saveFileDialog;

        }

    }
}
