using System.Management.Automation; //Windows PowerShell NameSpace

namespace AutoStart
{
    [Cmdlet(VerbsCommon.New, "AutoStart")]
    public class New_AutoStart : Cmdlet
    {
        #region Objects
        private AutoStartCommon NAS;
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
            NAS = new AutoStartCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                NAS.WriteReg(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", Name, Value);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}