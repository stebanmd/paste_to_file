using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PasteToFile
{
    public class PasteUtil
    {
        public static void PasteToFile(string path)
        {
            if (Clipboard.ContainsImage() || Clipboard.ContainsText() || Clipboard.ContainsFileDropList())
            {
                string baseTargetName = path + "\\" + ContextUtil.GetFileName();
                if (Clipboard.ContainsText())
                {
                    CreateTextFile(baseTargetName);
                }
                else if (Clipboard.ContainsImage())
                {
                    CreateImageFile(baseTargetName);
                }
                else
                {
                    CopyFilesToDirectory(baseTargetName);
                }
            }
            else
            {
                throw new Exception("Não há dados reconhecíveis na área de transferência.");
            }
        }

        private static void CreateTextFile(string target)
        {
            using (StreamWriter sw = new StreamWriter(target + ".txt", false, Encoding.UTF8))
            {
                try
                {
                    sw.Write(Clipboard.GetText());
                    sw.Flush();
                }
                catch (Exception ex)
                {
                    throw new Exception("Não foi possível gravar o arquivo. Detalhes: " + ex.Message);
                }
                finally
                {
                    sw.Close();
                }
            }
        }

        private static void CreateImageFile(string target)
        {
            try
            {
                Image imagem = Clipboard.GetImage();
                imagem.Save(target + ".png", ImageFormat.Png);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível gravar o arquivo. Detalhes: " + ex.Message);
            }
        }

        private static void CopyFilesToDirectory(string target)
        {
            DirectoryInfo dirDestino = new DirectoryInfo(target);

            if (!dirDestino.Exists)
                dirDestino.Create();

            string novo;
            foreach (string file in Clipboard.GetFileDropList())
            {
                if (File.Exists(file))
                {
                    FileInfo fi = new FileInfo(file);
                    novo = dirDestino.FullName + "\\" + fi.Name;
                    fi.CopyTo(novo, true);
                }
            }
        }
    }
}