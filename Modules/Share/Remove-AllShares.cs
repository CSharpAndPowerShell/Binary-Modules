using System.Management.Automation; //Windows PowerShell NameSpace

namespace Share
{
    [Cmdlet(VerbsCommon.Remove, "AllShares")]
    class Remove_AllShares : Cmdlet
    {
        #region Objects
        private ShareCommon RAS;
        private string[] ExcludeCollection;
        private string[] ExcludeDefault = { "ADMIN$", "C$", "IPC$" };
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre de los usuarios a excluir.")]
        public string[] Exclude
        {
            get { return ExcludeCollection; }
            set { ExcludeCollection = value; }
        }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            RAS = new ShareCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                RAS.RemoveAllShares(ExcludeDefault, ExcludeCollection);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}
