using System.Management.Automation; //Windows PowerShell NameSpace

namespace AutoStart
{
    [Cmdlet(VerbsCommon.New, "AutoStartOnce")]
    public class New_AutoStartOnce : Cmdlet
    {
        #region Objects
        private AutoStartCommon NASO;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre de la nueva propiedad del registro.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }
        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Valor de la propiedad, en este caso ruta al ejecutable.")]
        [ValidateNotNullOrEmpty]
        public string Value { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            NASO = new AutoStartCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                NASO.WriteReg(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce", Name, Value);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}