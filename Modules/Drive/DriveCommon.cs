using IWshRuntimeLibrary;
using System;

namespace Drive
{
    public class DriveCommon
    {
        #region Objects
        //Objeto de la clase relacionada con unidades de red
        private IWshNetwork_Class IWN;
        //Objeto de la clase relacionada con el ambiente de Windows
        private Shell32.Shell shell;
        #endregion
        #region Methods
        #region Drive
        public void RenameDrive(char letter, string name)
        {
            //Inicialización del objeto
            shell = new Shell32.Shell();
            //Renombra unidades locales y de red
            ((Shell32.Folder2)shell.NameSpace(letter + ":")).Self.Name = name;
        }
        public void SetDrives(string[] drives, bool nodrives, bool noviewondrive, bool disable)
        {
            //Establece valores que oculta o bloquea unidades de red
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
                    //Se eliminan ambas políticas
                    try
                    {
                        RegKey.DeleteValue("NoDrives");
                        RegKey.DeleteValue("NoViewOnDrive");
                    }
                    catch
                    {
                        RegKey.DeleteValue("NoViewOnDrive");
                    }
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
                            value += (int)Math.Pow(2, j);
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
        #endregion
        #region NetworkDrive
        public void NewNetworkDrive(char letter, string path, string user, string password, string name)
        {
            //Inicialización del objeto
            IWN = new IWshNetwork_Class();
            //Monta unidades de red
            if (user != null)
            {
                IWN.MapNetworkDrive(letter + ":", path, Type.Missing, user, password);
            }
            else
            {
                IWN.MapNetworkDrive(letter + ":", path);
            }
            //Crea un nombre si no fue dado
            if (name == null)
            {
                string[] names = path.Split(new string[] {@"\"}, StringSplitOptions.None);
                name = names[names.Length - 1];
            }
            //Se renombra la unidad de red
            RenameDrive(letter, name);
        }
        public void RemoveNetworkDrive(char letter)
        {
            //Inicialización del objeto
            IWN = new IWshNetwork_Class();
            //Desmonta unidades de red
            IWN.RemoveNetworkDrive(letter + ":", true);
        }
        #endregion
        #endregion
    }
}