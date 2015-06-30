using System.Management.Automation; //Windows PowerShell NameSpace

namespace Share
{
    //Define el nombre del Cmdlet
    [Cmdlet(VerbsCommon.Remove, "Share")]
    public class Remove_Share : Cmdlet
    {
        #region Objects
        ShareCommon RS;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del recurso compartido a eliminar.")]
        [ValidateNotNullOrEmpty]
        public string Sharename { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            RS = new ShareCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                RS.RemoveShare(Sharename);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}
