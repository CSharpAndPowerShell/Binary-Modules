using System.Management.Automation; //Windows PowerShell NameSpace

namespace Drive
{
    [Cmdlet(VerbsCommon.Rename, "Drive")]
    public class Rename_Drive : Cmdlet
    {
        #region Objects
        private DriveCommon RD;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Letra de la unidad.")]
        [ValidateNotNullOrEmpty]
        public char Letter { get; set; }
        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre de la unidad.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            RD = new DriveCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                RD.RenameDrive(Letter, Name);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}