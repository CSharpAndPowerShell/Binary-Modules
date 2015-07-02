using System.Management.Automation; //Windows PowerShell NameSpace

namespace UI
{
    //Define el nombre del Cmdlet
    [Cmdlet(VerbsCommon.Show, "MessageBox")]
    public class Show_MessageBox : Cmdlet
    {
        #region Objects
        private UICommon SMB;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del nuevo usuario.")]
        public string Message { get; set; }
        [Parameter(Position = 1, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Contraseña para el nuevo usuario.")]
        public string Title { get; set; }
        [Parameter(Position = 2, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Letra en la que se montará el 'HomeDirectory'.")]
        [ValidateSet("Asterisk", "Error", "Exclamation", "Hand", "Information", "None", "Question", "Stop", "Warning")]
        public string Icon { get; set; }
        [Parameter(Position = 3, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Letra en la que se montará el 'HomeDirectory'.")]
        [ValidateSet("AbortRetryIgnore", "OK", "OKCancel", "RetryCancel", "YesNo", "YesNoCancel")]
        public string Buttons { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            SMB = new UICommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                SMB.ShowMessageBox(Message, Title, Buttons, Icon);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}