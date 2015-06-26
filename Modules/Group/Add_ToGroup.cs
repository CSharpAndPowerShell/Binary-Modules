using System.Management.Automation; //Windows PowerShell NameSpace

namespace Group
{
    [Cmdlet(VerbsCommon.Add, "ToGroup")]
    public class Add_ToGroup : Cmdlet
    {
        #region Objects
        private GroupCommon ATG;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del nuevo usuario.")]
        [ValidateNotNullOrEmpty]
        [ValidateLength(1, 14)]
        public string Name { get; set; }
        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Grupo al que pertenecerá el nuevo usuario.")]
        [ValidateNotNullOrEmpty]
        public string Group { get; set; }
        #endregion
        
        protected override void BeginProcessing()
        {
            ATG = new GroupCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                ATG.AddToGroup(Name, Group);
            }
            catch
            {
                ATG.CloseConn(true, false, true);
            }
        }
        protected override void EndProcessing()
        {
            ATG.CloseConn(true, true, true);
        }
    }
}