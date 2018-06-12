using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PasteToFile
{
  public class Utils
  {
    public static readonly string MenuName = "Directory\\Background\\shell\\PasteToFile";

    public static readonly string MenuCommand = "Directory\\Background\\shell\\PasteToFile\\command";

    private static readonly string menuTexto = "Colar para arquivo: ";

    public static void CriaContexto(string fileName)
    {
      RegistryKey regmenu = null;
      RegistryKey regcmd = null;

      foreach (char c in fileName)
        if (Path.GetInvalidFileNameChars().Contains(c))
          throw new Exception("Nome informado possui caracteres inválidos");

      try
      {
        CheckSecurity();

        regmenu = Registry.ClassesRoot.CreateSubKey(MenuName);
        if (regmenu != null)
        {
          regmenu.SetValue(string.Empty, menuTexto + fileName);
          regmenu.SetValue("Position", "Top");
        }

        regcmd = Registry.ClassesRoot.CreateSubKey(MenuCommand);
        if (regcmd != null)
        {
          regcmd.SetValue(string.Empty, string.Concat(Assembly.GetExecutingAssembly().Location, " ", "%v"));
        }
      }
      finally
      {
        if (regmenu != null) regmenu.Close();
        if (regcmd != null) regcmd.Close();
      }
    }

    public static void RemoveContexto()
    {
      try
      {
        CheckSecurity();
        RegistryKey reg = Registry.ClassesRoot.OpenSubKey(MenuCommand);
        if (reg != null)
        {
          reg.Close();
          Registry.ClassesRoot.DeleteSubKey(MenuCommand);
        }

        reg = Registry.ClassesRoot.OpenSubKey(MenuName);
        if (reg != null)
        {
          reg.Close();
          Registry.ClassesRoot.DeleteSubKey(MenuName);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    private static void CheckSecurity()
    {
      RegistryPermission regPerm = new RegistryPermission(RegistryPermissionAccess.Write, "HKEY_CLASSES_ROOT\\" + MenuName);
      regPerm.AddPathList(RegistryPermissionAccess.Write, "HKEY_CLASSES_ROOT\\" + MenuCommand);
      regPerm.Demand();
    }

    public static string GetFileName()
    {
      RegistryKey reg = Registry.ClassesRoot.OpenSubKey(MenuName);
      string result = string.Empty;

      if (reg != null)
      {
        result = reg.GetValue(string.Empty, string.Empty).ToString();
        if (!string.IsNullOrWhiteSpace(result) && (result.Length > menuTexto.Length))
        {
          result = result.Substring(menuTexto.Length);
        }
      }

      return result;
    }

    public static void PasteToFile(string path)
    {
      if (Clipboard.ContainsImage() || Clipboard.ContainsText() || Clipboard.ContainsFileDropList())
      {
        if (Clipboard.ContainsText())
        {
          StreamWriter sw = new StreamWriter(path + "\\" + GetFileName() + ".txt", false, Encoding.UTF8);
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
            sw.Dispose();
          }
        }
        else if (Clipboard.ContainsImage())
        {
          try
          {
            Image imagem = Clipboard.GetImage();
            imagem.Save(path + "\\" + GetFileName() + ".png", ImageFormat.Png);
          }
          catch (Exception ex)
          {
            throw new Exception("Não foi possível gravar o arquivo. Detalhes: " + ex.Message);
          }
        }
        else
        {
          DirectoryInfo dirDestino = new DirectoryInfo(path + "\\" + GetFileName());
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
      else
      {
        throw new Exception("Não há dados na área de transferência.");
      }
    }
  }
}