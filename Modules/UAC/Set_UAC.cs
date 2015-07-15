using System.Management.Automation; //Windows PowerShell NameSpace

namespace UAC
{
    [Cmdlet(VerbsCommon.Set, "UAC")]
    public class Set_UAC : Cmdlet
    {
        #region Objects
        private UACCommon SUAC;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Valor boleano de la propiedad.")]
        [ValidateNotNullOrEmpty]
        public SwitchParameter Enable
        {
            get { return enable; }
            set { enable = value; }
        }
        private bool enable;
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Valor boleano de la propiedad.")]
        [ValidateNotNullOrEmpty]
        public SwitchParameter Disable
        {
            get { return disable; }
            set { disable = value; }
        }
        private bool disable;
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            SUAC = new UACCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                if (!(enable && disable))
                {
                    int value = 0;
                    if (enable)
                    {
                        value = 1;
                    }
                    if (disable)
                    {
                        value = 0;
                    }
                    SUAC.Set_UAC(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableLUA", value);
                    SUAC.Set_UAC(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ConsentPromptBehaviorAdmin", value);
                }
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}