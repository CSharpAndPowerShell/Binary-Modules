using System.Management.Automation; //Windows PowerShell NameSpace

namespace User
{
    [Cmdlet(VerbsCommon.Add, "ToGroup")]
    public class Add_ToGroup : Cmdlet
    {
        #region Objects
        private UserCommon ATG;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del nuevo usuario.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }
        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Grupo al que pertenecerá el nuevo usuario.")]
        [ValidateNotNullOrEmpty]
        public string Group { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            ATG = new UserCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                ATG.AddToGroup(Name, Group);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        protected override void EndProcessing()
        {
            ATG.CloseConn();
        }
        #endregion
    }
}
