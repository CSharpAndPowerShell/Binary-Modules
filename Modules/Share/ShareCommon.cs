/*
CSharpAndPowerShell Modules, tries to help Microsoft Windows admins to write automated scripts easier.
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
        private ArrayList GetShares(string[] Default = null, string[] Custom = null)
        {
            SelectQuery Query = new SelectQuery("Win32_Share");
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Query);
            ArrayList Shares = new ArrayList();
            foreach (ManagementObject Share in Searcher.Get())
            {
                Shares.Add(Share["Name"].ToString());
            }
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
        public void NewShare(string Sharename, string Path, string User, string Access, string Right, string ACL, string Description)
        {
            TestDir(Path);
            //Sharing folder
            ManagementBaseObject inParams = MC.GetMethodParameters("Create");
            inParams["Description"] = Description;
            inParams["Name"] = Sharename;
            inParams["Path"] = Path;
            inParams["Type"] = 0x0;
            inParams["MaximumAllowed"] = null;
            inParams["Password"] = null;
            inParams["Access"] = null;
            MC.InvokeMethod("Create", inParams, null);
            if (Right == null)
            {
                switch (Access)
                {
                    case "FullControl":
                        Right = Access;
                        break;
                    case "Change":
                        Right = "Write";
                        break;
                    case "Read":
                        Right = Access;
                        break;
                    default:
                        Right = "Read";
                        break;
                }
            }
            NewACE(Path, User, Right, ACL, false);
            AddSharePermissions(Sharename, User, Access);
        }
        public void NewACE(string Path, string User, string Right, string ACL, bool testDir = true)
        {
            //Crea el directorio en caso de que no exista
            if(testDir)
            {
                TestDir(Path);
            }
            //Control de Acceso (Permisos NTFS)
            //Objetos
            DirectoryInfo dInfo = new DirectoryInfo(Path);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            AccessControlType ACT;
            FileSystemRights FSR;
            //Asignación de ACL
            switch (ACL)
            {
                case "Allow":
                    ACT = AccessControlType.Allow;
                    break;
                case "Deny":
                    ACT = AccessControlType.Deny;
                    break;
                default:
                    ACT = AccessControlType.Allow;
                    break;
            }
            //Asignación de derechos
            switch (Right)
            {
                case "AppendData":
                    FSR = FileSystemRights.AppendData;
                    break;
                case "ChangePermissions":
                    FSR = FileSystemRights.ChangePermissions;
                    break;
                case "CreateDirectories":
                    FSR = FileSystemRights.CreateDirectories;
                    break;
                case "CreateFiles":
                    FSR = FileSystemRights.CreateFiles;
                    break;
                case "Delete":
                    FSR = FileSystemRights.Delete;
                    break;
                case "DeleteSubdirectoriesAndFiles":
                    FSR = FileSystemRights.DeleteSubdirectoriesAndFiles;
                    break;
                case "ExecuteFile":
                    FSR = FileSystemRights.ExecuteFile;
                    break;
                case "FullControl":
                    FSR = FileSystemRights.FullControl;
                    break;
                case "ListDirectory":
                    FSR = FileSystemRights.ListDirectory;
                    break;
                case "Modify":
                    FSR = FileSystemRights.Modify;
                    break;
                case "Read":
                    FSR = FileSystemRights.Read;
                    break;
                case "ReadAndExecute":
                    FSR = FileSystemRights.ReadAndExecute;
                    break;
                case "ReadAttributes":
                    FSR = FileSystemRights.ReadAttributes;
                    break;
                case "ReadData":
                    FSR = FileSystemRights.ReadData;
                    break;
                case "ReadExtendedAttributes":
                    FSR = FileSystemRights.ReadExtendedAttributes;
                    break;
                case "ReadPermissions":
                    FSR = FileSystemRights.ReadPermissions;
                    break;
                case "Synchronize":
                    FSR = FileSystemRights.Synchronize;
                    break;
                case "TakeOwnership":
                    FSR = FileSystemRights.TakeOwnership;
                    break;
                case "Traverse":
                    FSR = FileSystemRights.Traverse;
                    break;
                case "Write":
                    FSR = FileSystemRights.Write;
                    break;
                case "WriteAttributes":
                    FSR = FileSystemRights.WriteAttributes;
                    break;
                case "WriteData":
                    FSR = FileSystemRights.WriteData;
                    break;
                case "WriteExtendedAttributes":
                    FSR = FileSystemRights.WriteExtendedAttributes;
                    break;
                default:
                    FSR = FileSystemRights.Read;
                    break;
            }
            //Aplicando cambios
            dSecurity.AddAccessRule(new FileSystemAccessRule(User, FSR, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, ACT));
            dInfo.SetAccessControl(dSecurity);
        }
        public void AddSharePermissions(string Sharename, string User, string Access)
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
            switch (Access)
            {
                case "Read":
                    ACE["AccessMask"] = 1179817; //Read
                    break;
                case "Change":
                    ACE["AccessMask"] = 1245631; //Change
                    break;
                case "FullControl":
                    ACE["AccessMask"] = 2032127; //FullControl
                    break;
                default:
                    ACE["AccessMask"] = 1179817; //Read
                    break;
            }
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
                DACL = new ManagementBaseObject[] { ACE };
            }
            else
            {
                //Se agregan los permisos adicionales
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
            //Inicializando el objeto
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