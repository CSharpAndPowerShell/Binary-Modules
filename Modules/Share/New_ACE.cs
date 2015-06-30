using System.Management.Automation; //Windows PowerShell NameSpace

namespace Share
{
    //Define el nombre del Cmdlet
    [Cmdlet(VerbsCommon.New, "ACE")]
    public class New_ACE : Cmdlet
    {
        #region Objects
        private ShareCommon NA;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Ruta a la carpeta.")]
        [ValidateNotNullOrEmpty]
        public string Path { get; set; }
        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del usuario o grupo al que se le modificará el acceso.")]
        [ValidateNotNullOrEmpty]
        public string User { get; set; }
        [Parameter(Position = 2, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nivel de acceso que se le otorgará al usuario o grupo.")]
        [ValidateSet("ListDirectory", "ReadData", "WriteData", "CreateFiles", "CreateDirectories", "AppendData", "ReadExtendedAttributes", "WriteExtendedAttributes", "Traverse", "ExecuteFile", "DeleteSubdirectoriesAndFiles", "ReadAttributes", "WriteAttributes", "Write", "Delete", "ReadPermissions", "Read", "ReadAndExecute", "Modify", "ChangePermissions", "TakeOwnership", "Synchronize", "FullControl")]
        public string Right { get; set; }
        [Parameter(Position = 3, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Tipo de ACL que se le otorgará al usuario o grupo.")]
        [ValidateSet("Allow", "Deny")]
        public string ACL { get; set; }
        #endregion
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
    }
}
