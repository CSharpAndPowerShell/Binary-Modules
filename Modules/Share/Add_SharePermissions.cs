using System.Management.Automation; //Windows PowerShell NameSpace

namespace Share
{
    //Define el nombre del Cmdlet
    [Cmdlet(VerbsCommon.Add, "SharePermissions")]
    public class Add_SharePermissions : Cmdlet
    {
        #region Objects
        private ShareCommon ASP;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del recurso compartido.")]
        [ValidateNotNullOrEmpty]
        public string Sharename { get; set; }
        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del usuario o grupo al que le será compatido el recurso.")]
        [ValidateNotNullOrEmpty]
        public string User { get; set; }
        [Parameter(Position = 2, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nivel de acceso que se le otorgará al usuario o grupo.")]
        [ValidateSet("FullControl", "Change", "Read")]
        public string Access { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            ASP = new ShareCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                ASP.AddSharePermission(Sharename,User,Access);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}
