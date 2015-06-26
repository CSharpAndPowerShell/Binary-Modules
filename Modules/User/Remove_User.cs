using System.Management.Automation; //Windows PowerShell NameSpace

namespace Group
{
    [Cmdlet(VerbsCommon.Remove, "User")]
    public class Remove_User : Cmdlet
    {
        #region Objects
        private UserCommon RU;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del usuario a eliminar.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            RU = new UserCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                RU.RemoveUser(Name);
            }
            catch
            {
                RU.CloseConn(true, true, false);
            }
        }
        protected override void EndProcessing()
        {
            RU.CloseConn(true, true, false);
        }
        #endregion
    }
}