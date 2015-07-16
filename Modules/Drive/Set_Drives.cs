using System.Management.Automation; //Windows PowerShell NameSpace

namespace Drive
{
    [Cmdlet(VerbsCommon.Set, "Drives")]
    public class Set_Drives : Cmdlet
    {
        #region Objects
        private DriveCommon SD;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Valor boleano de la propiedad.")]
        [ValidateNotNullOrEmpty]
        public SwitchParameter NoDrives
        {
            get { return nodrives; }
            set { nodrives = value; }
        }
        private bool nodrives;
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Valor boleano de la propiedad.")]
        [ValidateNotNullOrEmpty]
        public SwitchParameter NoViewOnDrive
        {
            get { return noviewondrive; }
            set { noviewondrive = value; }
        }
        private bool noviewondrive;
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Valor boleano de la propiedad.")]
        [ValidateNotNullOrEmpty]
        public SwitchParameter Disable
        {
            get { return disable; }
            set { disable = value; }
        }
        private bool disable;
        [Parameter(Position = 1, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre de los usuarios a excluir.")]
        public string[] Drives
        {
            get { return drives; }
            set { drives = value; }
        }
        private string[] drives;
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            SD = new DriveCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                SD.SetDrives(drives, nodrives, noviewondrive, disable);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}