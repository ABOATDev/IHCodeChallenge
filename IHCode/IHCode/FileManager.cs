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

        public List<CodeFile> Files { get; } = new List<CodeFile>();

        public bool AddFile(string fileName)
        {

            if (Files.Where(f => f.FullPath == fileName).Any())
            {

                return false;

            }

            Files.Add(new CodeFile(fileName));

            return true;

        }

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

            foreach (string fileName in files)
            {

                AddFile(fileName);

            }

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

        public bool RenameFile(string oldFileName, string newFileName)
        {
            
            if (!File.Exists(oldFileName))
            {

                return false;

            }

            try
            {

                File.Move(oldFileName, newFileName);

            } catch { return false; }

            return true;

        }

        public bool DeleteFile(string filename)
        {
            bool etat = false;
            try
            {
               File.Delete(@"" + filename);
               etat = true;
              return etat;
            }
            catch { return etat; }
        }


        public string InfoFile(string patch)
        {
            string typefile = System.IO.Path.GetExtension(patch).ToUpper();
            FileInfo f = new FileInfo(patch);
            string sizefile = f.Length.ToString();

            return typefile + " | " + Encoder(patch) + " | " + sizefile + " octets";
        }

        public string Encoder(string CheminFichier)
        {
            string enc;

            FileStream file = new FileStream(CheminFichier, FileMode.Open, FileAccess.Read, FileShare.Read);
            if(file.CanSeek)
            {
                byte[] bom = new byte[4]; // Get the byte-order mark, if there is one
                file.Read(bom, 0, 4);
                if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) // utf-8
                {
                    enc = "UTF8";
                }
                else if ((bom[0] == 0xff && bom[1] == 0xfe) || // ucs-2le, ucs-4le, and ucs-16le
                 (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)) // ucs-4
                {
                    enc = "Unicode";
                }
                else if (bom[0] == 0xfe && bom[1] == 0xff) // utf-16 and ucs-2
                {
                    enc = "UTF16";
                }
                else // ANSI, Default
                {
                    enc = "ANSI";
                }
                // Now reposition the file cursor back to the start of the file
                file.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                // The file cannot be randomly accessed, so you need to decide what to set the default to
                // based on the data provided. If you're expecting data from a lot of older applications,
                // default your encoding to Encoding.ASCII. If you're expecting data from a lot of newer
                // applications, default your encoding to Encoding.Unicode. Also, since binary files are
                // single byte-based, so you will want to use Encoding.ASCII, even though you'll probably
                // never need to use the encoding then since the Encoding classes are really meant to get
                // strings from the byte array that is the file.

                enc = Encoding.Default.ToString();
            }

            return enc;


        }

    }

    public class CodeFile
    {

        public CodeFile(string filePath)
        {

            this.FullPath = filePath;

        }

        public string FullPath { get; set; } = string.Empty;

        public string FriendlyName { get => System.IO.Path.GetFileName(FullPath); }

    }

    static class Constants
    {
        public static string JAVASCRIPT_FILE_SEARCH_PATTERN = "*.js";

    }

}
