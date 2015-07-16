using System;

namespace Drive
{
    public class DriveCommon
    {
        public void RenameDrive(char letter, string name) 
        {
            //Renombra unidades locales y de red
            Shell32.Shell shell = new Shell32.Shell();
            ((Shell32.Folder2)shell.NameSpace(letter + ":")).Self.Name = name;
        }
        public void SetDrives(string[] drives, bool nodrives, bool noviewondrive, bool disable)
        {
            Microsoft.Win32.RegistryKey RegKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\Currentversion\Policies", true);
            if (disable)
            {
                //Eliminar llave explorer de policies
                RegKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\Currentversion\Policies\Explorer", true);
                if (nodrives)
                {
                    RegKey.DeleteValue("NoDrives");
                }
                else if (noviewondrive)
                {
                    RegKey.DeleteValue("NoViewOnDrive");
                }
                else
                {
                    RegKey.DeleteValue("NoDrives");
                    RegKey.DeleteValue("NoViewOnDrive");
                }
            }
            else
            {
                int value = 0;
                //Calculando el valor de la letra
                string[] Letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                for (int i = 0; i < drives.Length; i++)
                {
                    for (int j = 0; j < Letters.Length; j++)
                    {
                        if (drives[i].Equals(Letters[j]))
                        {
                            value += (int) Math.Pow(2, j);
                        }
                    }
                }
                //Escribiendo los cambios en el registro
                if (nodrives)
                {
                    try
                    {
                        Microsoft.Win32.Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Currentversion\Policies\Explorer", "NoDrives", value, Microsoft.Win32.RegistryValueKind.DWord);
                    }
                    catch
                    {
                        RegKey.CreateSubKey("Explorer");
                        Microsoft.Win32.Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Currentversion\Policies\Explorer", "NoDrives", value, Microsoft.Win32.RegistryValueKind.DWord);
                    }
                }
                else if (noviewondrive)
                {
                    try
                    {
                        Microsoft.Win32.Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Currentversion\Policies\Explorer", "NoViewOnDrive", value, Microsoft.Win32.RegistryValueKind.DWord);
                    }
                    catch
                    {
                        RegKey.CreateSubKey("Explorer");
                        Microsoft.Win32.Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Currentversion\Policies\Explorer", "NoViewOnDrive", value, Microsoft.Win32.RegistryValueKind.DWord);
                    }
                }
            }
        }
    }
}
