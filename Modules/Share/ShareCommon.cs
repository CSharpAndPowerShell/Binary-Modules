using System.Collections;
using System.IO;
using System.Management;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Share
{
    public class ShareCommon
    {
        private ManagementClass MC = new ManagementClass("win32_share");
        private ManagementObject Share;
        public void NewShare(string Sharename, string Path, string User, string Access, string Right, string ACL, string Description)
        {
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
            NewACE(Path, User, Right, ACL);
            AddSharePermission(Sharename, User, Access);
        }
        public void NewACE(string Path, string User, string Right, string ACL)
        {
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
        public void AddSharePermission(string Sharename, string User, string Access)
        {
            //Agrega permisos a carpetas compartidas
            //User
            NTAccount ntAccount = new NTAccount(User);
            //SID
            SecurityIdentifier userSID = (SecurityIdentifier)ntAccount.Translate(typeof(SecurityIdentifier));
            byte[] utenteSIDArray = new byte[userSID.BinaryLength];
            userSID.GetBinaryForm(utenteSIDArray, 0);
            //Trustee
            ManagementObject userTrustee = new ManagementClass(new ManagementPath("Win32_Trustee"), null);
            userTrustee["Name"] = User;
            userTrustee["SID"] = utenteSIDArray;
            //ACE
            ManagementObject userACE = new ManagementClass(new ManagementPath("Win32_Ace"), null);
            switch (Access)
            {
                case "Read":
                    userACE["AccessMask"] = 1179817; //Read
                    break;
                case "Change":
                    userACE["AccessMask"] = 1245631; //Change
                    break;
                case "FullControl":
                    userACE["AccessMask"] = 2032127; //FullControl
                    break;
                default:
                    userACE["AccessMask"] = 1179817; //Read
                    break;
            }
            userACE["AceFlags"] = AceFlags.ObjectInherit | AceFlags.ContainerInherit;
            userACE["AceType"] = AceType.AccessAllowed;
            userACE["Trustee"] = userTrustee;
            ManagementObject userSecurityDescriptor = new ManagementClass(new ManagementPath("Win32_SecurityDescriptor"), null);
            userSecurityDescriptor["ControlFlags"] = 4; //SE_DACL_PRESENT 
            userSecurityDescriptor["DACL"] = new object[] { userACE };
            //Actualizando permisos
            Share = new ManagementObject(MC.Path + ".Name='" + Sharename + "'");
            Share.InvokeMethod("SetShareInfo", new object[] { null, null, userSecurityDescriptor });
        }
        public void RemoveShare(string Sharename)
        {
            Share = new ManagementObject(MC.Path + ".Name='" + Sharename + "'");
            Share.Delete();
        }
        public ArrayList GetShares(string[] Default = null, string[] Custom = null)
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
        public void RemoveAllShares(string[] Default, string[] Custom)
        {
            ArrayList Shares = GetShares(Default, Custom);
            foreach (string Share in Shares)
            {
                RemoveShare(Share);
            }
        }
    }
}