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
      string fileName = Utils.GetFileName();
      string diretorio = string.Join(" ", args);

      if (string.IsNullOrWhiteSpace(diretorio) || string.IsNullOrWhiteSpace(fileName))
      {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new frmMain(fileName));
      }
      else
      {
        try
        {
          Utils.PasteToFile(diretorio);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
    }
  }
}