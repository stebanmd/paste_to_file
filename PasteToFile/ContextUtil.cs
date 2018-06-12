using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;

namespace PasteToFile
{
    public class ContextUtil
    {
        private const string _menuName = "Directory\\Background\\shell\\PasteToFile";
        private const string _menuCommand = "Directory\\Background\\shell\\PasteToFile\\command";
        private const string _menuText = "Colar para arquivo: ";

        public static void CreateContext(string fileName)
        {
            RegistryKey regmenu = null;
            RegistryKey regcmd = null;

            foreach (char c in fileName)
                if (Path.GetInvalidFileNameChars().Contains(c))
                    throw new Exception("Nome informado possui caracteres inválidos");

            try
            {
                CheckSecurity();

                regmenu = Registry.ClassesRoot.CreateSubKey(_menuName);
                if (regmenu != null)
                {
                    regmenu.SetValue(string.Empty, _menuText + fileName);
                    regmenu.SetValue("Position", "Top");
                }

                regcmd = Registry.ClassesRoot.CreateSubKey(_menuCommand);
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

        public static void RemoveContext()
        {
            try
            {
                CheckSecurity();
                RegistryKey reg = Registry.ClassesRoot.OpenSubKey(_menuCommand);
                if (reg != null)
                {
                    reg.Close();
                    Registry.ClassesRoot.DeleteSubKey(_menuCommand);
                }

                reg = Registry.ClassesRoot.OpenSubKey(_menuName);
                if (reg != null)
                {
                    reg.Close();
                    Registry.ClassesRoot.DeleteSubKey(_menuName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void CheckSecurity()
        {
            RegistryPermission regPerm = new RegistryPermission(RegistryPermissionAccess.Write, "HKEY_CLASSES_ROOT\\" + _menuName);
            regPerm.AddPathList(RegistryPermissionAccess.Write, "HKEY_CLASSES_ROOT\\" + _menuCommand);
            regPerm.Demand();
        }

        public static string GetFileName()
        {
            RegistryKey reg = Registry.ClassesRoot.OpenSubKey(_menuName);
            string result = string.Empty;

            if (reg != null)
            {
                result = reg.GetValue(string.Empty, string.Empty).ToString();
                if (!string.IsNullOrWhiteSpace(result) && (result.Length > _menuText.Length))
                {
                    result = result.Substring(_menuText.Length);
                }
            }

            return result;
        }
    }
}