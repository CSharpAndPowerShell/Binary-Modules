using System.Management.Automation; //Windows PowerShell NameSpace

namespace Group
{
    [Cmdlet(VerbsCommon.New, "Group")]
    public class New_User : Cmdlet
    {
        #region Objects
        private GroupCommon NG;
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
            NG = new GroupCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                NG.NewGroup(Name, Description);
            }
            catch
            {
                NG.CloseConn(true, false, true);
            }
        }
        protected override void EndProcessing()
        {
            NG.CloseConn(true, false, true);
        }
        #endregion
    }
}