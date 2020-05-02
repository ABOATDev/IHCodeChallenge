using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace IHCode
{

    public class FileManager
    {

        public List<string> Files { get; } = new List<string>();

        public string CurrentDirectory { get; set; } = string.Empty;

        public bool OpenDirectory(string directory)
        {

            if (!(Directory.Exists(directory)))
            {
                return false;
            }
            
            Files.Clear();

            string[] files = Directory.GetFiles(directory, Constants.JAVASCRIPT_FILE_SEARCH_PATTERN);

            if (files.Length == 0)
            {

                return false;

            }

            Files.AddRange(files);

            return true;

        }

        public bool SaveFile(string fileName, string content)
        {

            if (!File.Exists(fileName))
            {

                return false;

            }

            try
            {

                // Ré-écriture du fichier
                using (StreamWriter sr = new StreamWriter(fileName))
                {
                    sr.Write(content);
                }

            } catch { return false; }

            return true;

        }


        public bool RenameFile(string filename, string newfilename)
        {
            //File.Rename("C:\MonFichier.txt", "C:\MonFichierRenommer.txt");
            //return true;
        }



    }

    static class Constants
    {

        public static string JAVASCRIPT_FILE_SEARCH_PATTERN = "*.js";

    }

}
