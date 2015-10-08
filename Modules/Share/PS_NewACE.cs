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

using System.Management.Automation; //Windows PowerShell NameSpace
using System.Security.AccessControl;

namespace Share
{
    //Define el nombre del Cmdlet
    [Cmdlet(VerbsCommon.New, "ACE")]
    public class PS_NewACE : Cmdlet
    {
        #region Objects
        private ShareCommon NA;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Ruta a la carpeta.")]
        [ValidateNotNullOrEmpty]
        public string Path { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Nombre del usuario o grupo al que se le modificará el acceso.")]
        [ValidateNotNullOrEmpty]
        public string User { get; set; }

        [Parameter(Position = 2, Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Nivel de acceso que se le otorgará al usuario o grupo.")]
        [ValidateSet("ListDirectory", "ReadData", "WriteData", "CreateFiles",
            "CreateDirectories", "AppendData", "ReadExtendedAttributes",
            "WriteExtendedAttributes", "Traverse", "ExecuteFile",
            "DeleteSubdirectoriesAndFiles", "ReadAttributes", "WriteAttributes",
            "Write", "Delete", "ReadPermissions", "Read", "ReadAndExecute", "Modify",
            "ChangePermissions", "TakeOwnership", "Synchronize", "FullControl")]
        public FileSystemRights Right { get; set; }

        [Parameter(Position = 3, Mandatory = false, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Tipo de ACL que se le otorgará al usuario o grupo.")]
        [ValidateSet("Allow", "Deny")]
        public AccessControlType ACL { get; set; }
        #endregion

        #region Methods
        protected override void BeginProcessing()
        {
            NA = new ShareCommon();
        }

        protected override void ProcessRecord()
        {
            try
            {
                NA.NewACE(Path, User, Right, ACL);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}
