using System;
using System.Windows.Forms;

namespace PasteToFile
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            string fileName = ContextUtil.GetFileName();
            string diretory = string.Join(" ", args);

            if (string.IsNullOrWhiteSpace(diretory) || string.IsNullOrWhiteSpace(fileName))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(fileName));
            }
            else
            {
                try
                {
                    PasteUtil.PasteToFile(diretory);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}