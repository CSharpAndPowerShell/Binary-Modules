using System.Management.Automation; //Windows PowerShell NameSpace

namespace User
{
    [Cmdlet(VerbsCommon.Remove, "Group")]
    public class Remove_Group : Cmdlet
    {
        #region Objects
        private UserCommon RG;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del grupo a eliminar.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            RG = new UserCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                RG.RemoveGroup(Name);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        protected override void EndProcessing()
        {
            RG.CloseConn();
        }
        #endregion
    }
}