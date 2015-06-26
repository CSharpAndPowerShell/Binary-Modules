using System.Management.Automation; //Windows PowerShell NameSpace

namespace Group
{
    [Cmdlet(VerbsCommon.Remove, "Group")]
    public class Remove_User : Cmdlet
    {
        #region Objects
        private GroupCommon RG;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del grupo a eliminar.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }
        #endregion
        protected override void BeginProcessing()
        {
            RG = new GroupCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                RG.RemoveGroup(Name);
            }
            catch
            {
                RG.CloseConn(true, false, true);
            }
        }
        protected override void EndProcessing()
        {
            RG.CloseConn(true, false, true);
        }
    }
}