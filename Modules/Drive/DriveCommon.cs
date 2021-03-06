﻿/*
CSharpAndPowerShell Modules, tries to help Microsoft Windows admins
to write automated scripts easier.
Copyright(C) 2015  Cristopher Robles Ríos

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.If not, see<http://www.gnu.org/licenses/>.

*/

using IWshRuntimeLibrary;
using System;

namespace Drive
{
    public class DriveCommon : Utils.Registry
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
            Microsoft.Win32.RegistryKey RegKey =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\Currentversion\Policies", true);
            string Key = @"SOFTWARE\Microsoft\Windows\Currentversion\Policies\Explorer";
            if (disable)
            {
                //Eliminar llave explorer de policies
                if (nodrives)
                {
                    DeleteReg(Key,"NoDrives");
                }
                else if (noviewondrive)
                {
                    DeleteReg(Key, "NoViewOnDrive");
                }
                else
                {
                    //Se eliminan ambas políticas
                    try
                    {
                        DeleteReg(Key, "NoDrives");
                        DeleteReg(Key, "NoViewOnDrive");
                    }
                    catch
                    {
                        DeleteReg(Key, "NoViewOnDrive");
                    }
                }
            }
            else
            {
                //Calculando el valor de la letra
                int value = 0;
                string[] Letters = { "A", "B", "C", "D", "E", "F", "G",
                    "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q",
                    "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
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
                        RegKey.CreateSubKey("Explorer");
                    }
                    catch
                    {
                        // Nada que hacer
                    }
                    WriteReg(Key, "NoDrives", value, Microsoft.Win32.RegistryValueKind.DWord);
                }
                else if (noviewondrive)
                {
                    try
                    {
                        RegKey.CreateSubKey("Explorer");
                    }
                    catch
                    {
                        // Nada que hacer
                    }
                    WriteReg(Key, "NoViewOnDrive", value, Microsoft.Win32.RegistryValueKind.DWord);
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