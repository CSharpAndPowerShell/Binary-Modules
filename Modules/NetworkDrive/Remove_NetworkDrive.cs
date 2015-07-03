using System.Management.Automation; //Windows PowerShell NameSpace

namespace NetworkDrive
{
    [Cmdlet(VerbsCommon.Remove, "NetworkDrive")]
    public class Remove_NetworkDrive : Cmdlet
    {
        #region Objects
        private NetworkDriveCommon RND;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Letra a desmontar.")]
        [ValidateNotNullOrEmpty]
        public char Letter { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            RND = new NetworkDriveCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                RND.RemoveNetworkDrive(Letter);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}
