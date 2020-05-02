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

        private static FolderBrowserDialog GetNewParametizedDialog()
        {

            FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.ShowNewFolderButton = true;
            dialog.RootFolder = Environment.SpecialFolder.DesktopDirectory;
            dialog.Description = "Open workspace.";

            return dialog;

        }

    }
}
