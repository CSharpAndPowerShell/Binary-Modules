using System.Management.Automation; //Windows PowerShell NameSpace

namespace Share
{
    //Define el nombre del Cmdlet
    [Cmdlet(VerbsCommon.New, "Share")]
    public class New_Share : Cmdlet
    {
        #region Objects
        private ShareCommon NS;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del nuevo recurso compartido.")]
        [ValidateNotNullOrEmpty]
        public string Sharename { get; set; }
        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Ruta a la carpeta a compartir.")]
        [ValidateNotNullOrEmpty]
        public string Path { get; set; }
        [Parameter(Position = 2, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del usuario o grupo al que le será compatido el recurso.")]
        [ValidateNotNullOrEmpty]
        public string User { get; set; }
        [Parameter(Position = 3, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nivel de acceso que se le otorgará al usuario o grupo.")]
        [ValidateSet("FullControl", "Change", "Read")]
        public string Access { get; set; }
        [Parameter(Position = 4, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nivel de acceso que se le otorgará al usuario o grupo.")]
        [ValidateSet("ListDirectory", "ReadData", "WriteData", "CreateFiles", "CreateDirectories", "AppendData", "ReadExtendedAttributes", "WriteExtendedAttributes", "Traverse", "ExecuteFile", "DeleteSubdirectoriesAndFiles", "ReadAttributes", "WriteAttributes", "Write", "Delete", "ReadPermissions", "Read", "ReadAndExecute", "Modify", "ChangePermissions", "TakeOwnership", "Synchronize", "FullControl")]
        public string Right { get; set; }
        [Parameter(Position = 5, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Tipo de ACL que se le otorgará al usuario o grupo.")]
        [ValidateSet("Allow", "Deny")]
        public string ACL { get; set; }
        [Parameter(Position = 6, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Descripción del recurso compartido")]
        public string Description { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            NS = new ShareCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                NS.NewShare(Sharename, Path, User, Access, Right, ACL, Description);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}