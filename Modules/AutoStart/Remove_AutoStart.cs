using System.Management.Automation; //Windows PowerShell NameSpace

namespace AutoStart
{
    [Cmdlet(VerbsCommon.Remove, "AutoStart")]
    public class Remove_AutoStart : Cmdlet
    {
        #region Objects
        private AutoStartCommon RAS;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre de la nueva propiedad del registro.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            RAS = new AutoStartCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                RAS.DeleteReg(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", Name);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}