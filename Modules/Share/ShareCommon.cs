/*
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

using System;
using System.Collections;
using System.IO;
using System.Management;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Share
{
    public class ShareCommon
    {
        #region Objects
        private ManagementClass MC = new ManagementClass("win32_share");
        private ManagementObject Share;
        #endregion
        
        #region Methods
        private void TestDir(string Path)
        {
            //Comprueba la existencia del directorio, si no existe lo crea
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        private ArrayList GetShares(string[] Default = null,
            string[] Custom = null)
        {
            // Devuelve una lista de los recursos compartidos

            // Se hace la consulta y se obtienen los usuarios
            SelectQuery Query = new SelectQuery("Win32_Share");

            // Se crea un objeto buscador
            ManagementObjectSearcher Searcher =
                new ManagementObjectSearcher(Query);

            // Lista para almacenar los nombres de los recursos compartidos
            ArrayList Shares = new ArrayList();

            // Se agregan los nombres al ArrayList
            foreach (ManagementObject Share in Searcher.Get())
            {
                Shares.Add(Share["Name"].ToString());
            }

            // Se aplican los filtros a la lista
            foreach (var item in Default)
            {
                Shares.Remove(item);
            }

            if (Custom != null)
            {
                foreach (var item in Custom)
                {
                    Shares.Remove(item);
                }
            }
            return Shares;
        }

        public void NewShare(string Sharename, string Path, string User,
            AccessType Access, FileSystemRights Right,
            AccessControlType ACL, string Description)
        {
            // Comprobando que existe la carpeta, sino se crea
            TestDir(Path);

            // Compartiendo recurso
            // Obtiendo parametros del método "Create"
            ManagementBaseObject Parameters = MC.GetMethodParameters("Create");

            // Estableciendo valores a los parámetros
            Parameters["Description"] = Description;
            Parameters["Name"] = Sharename;
            Parameters["Path"] = Path;
            Parameters["Type"] = 0x0;
            Parameters["MaximumAllowed"] = null;
            Parameters["Password"] = null;
            Parameters["Access"] = null;

            // Se ejecuta el método y se 
            MC.InvokeMethod("Create", Parameters, null);

            // Aplicando permisos
            if (Right.ToString() == null)
            {
                switch (Access)
                {
                    case AccessType.FullControl:
                        Right = FileSystemRights.FullControl;
                        break;
                    case AccessType.Change:
                        Right = FileSystemRights.Write;
                        break;
                    case AccessType.Read:
                        Right = FileSystemRights.Read;
                        break;
                    default:
                        Right = FileSystemRights.Read;
                        break;
                }
            }

            // Aplicando permisos NTFS
            NewACE(Path, User, Right, ACL);

            // Aplicando Permisos SMB
            AddSharePermissions(Sharename, User, Access);
        }

        public void NewACE(string Path, string User, FileSystemRights Right,
            AccessControlType ACL)
        {
            //Crea el directorio en caso de que no exista
            TestDir(Path);

            //Control de Acceso (Permisos NTFS)
            //Objetos
            DirectoryInfo dInfo = new DirectoryInfo(Path);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            
            // Validando
            if (ACL.ToString() == null)
            {
                ACL = AccessControlType.Allow;
            }

            //Aplicando cambios
            dSecurity.AddAccessRule(new FileSystemAccessRule(User, Right, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, ACL));
            dInfo.SetAccessControl(dSecurity);
        }

        public enum AccessType
        {
            // Permisos de accesos a los recursos SMB
            Read = 1179817,
            Change = 1245631,
            FullControl = 2032127
        }

        public void AddSharePermissions(string Sharename,
            string User, AccessType Access)
        {
            //Agrega permisos a carpetas compartidas
            
            //User
            NTAccount ntAccount = new NTAccount(User);
            
            //SID
            SecurityIdentifier userSID = (SecurityIdentifier)ntAccount.Translate(typeof(SecurityIdentifier));
            byte[] SIDArray = new byte[userSID.BinaryLength];
            userSID.GetBinaryForm(SIDArray, 0);
            
            //Trustee
            ManagementObject Trustee = new ManagementClass(new ManagementPath("Win32_Trustee"), null);
            Trustee["Name"] = User;
            Trustee["SID"] = SIDArray;
            
            //ACE
            ManagementObject ACE = new ManagementClass(new ManagementPath("Win32_Ace"), null);
            ACE["AccessMask"] = Access;
            ACE["AceFlags"] = AceFlags.ObjectInherit | AceFlags.ContainerInherit;
            ACE["AceType"] = AceType.AccessAllowed;
            ACE["Trustee"] = Trustee;

            //Obteniendo permisos actuales
            ManagementObject Win32LogicalSecuritySetting = new ManagementObject(@"\\localhost\root\cimv2:Win32_LogicalShareSecuritySetting.Name='" + Sharename + "'");
            ManagementBaseObject Return = Win32LogicalSecuritySetting.InvokeMethod("GetSecurityDescriptor", null, null);
            ManagementBaseObject SecurityDescriptor = Return.Properties["Descriptor"].Value as ManagementBaseObject;
            ManagementBaseObject[] DACL = SecurityDescriptor["DACL"] as ManagementBaseObject[];

            if (DACL == null)
            {
                // Se crean los nuevos permisos
                DACL = new ManagementBaseObject[] { ACE };
            }
            else
            {
                // Se agregan los permisos adicionales
                Array.Resize(ref DACL, DACL.Length + 1);
                DACL[DACL.Length - 1] = ACE;
            }

            SecurityDescriptor["DACL"] = DACL;
            SecurityDescriptor["ControlFlags"] = 4; //SE_DACL_PRESENT

            //Actualizando permisos
            Share = new ManagementObject(MC.Path + ".Name='" + Sharename + "'");
            Share.InvokeMethod("SetShareInfo", new object[] { null, null, SecurityDescriptor });
        }

        public void RemoveShare(string Sharename)
        {
            //Inicializando el objeto y se referencia al recurso compartido
            Share = new ManagementObject(MC.Path + ".Name='" + Sharename + "'");
            //Eliminando el recurso compartido
            Share.Delete();
        }

        public void RemoveAllShares(string[] Default, string[] Custom)
        {
            ArrayList Shares = GetShares(Default, Custom);
            foreach (string Share in Shares)
            {
                RemoveShare(Share);
            }
        }
        #endregion
    }
}