using System.Management.Automation; //Windows PowerShell NameSpace

namespace User
{
    [Cmdlet(VerbsCommon.New, "Group")]
    public class New_Group : Cmdlet
    {
        #region Objects
        private UserCommon NG;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del nuevo grupo.")]
        [ValidateNotNullOrEmpty]
        [ValidateLength(1, 14)]
        public string Name { get; set; }
        [Parameter(Position = 1, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Descripción del nuevo grupo.")]
        public string Description { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            NG = new UserCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                NG.NewGroup(Name, Description);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        protected override void EndProcessing()
        {
            NG.CloseConn();
        }
        #endregion
    }
}