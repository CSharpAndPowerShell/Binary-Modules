using System.Management.Automation; //Windows PowerShell NameSpace

namespace User
{
    [Cmdlet(VerbsCommon.Remove, "AllUsers")]
    public class Remove_AllUsers : Cmdlet
    {
        #region Objects
        private UserCommon RAU;
        private string[] ExcludeCollection;
        private string[] ExcludeDefault = { "Administrator", "Administrador", "Invitado", "Guest", "DefaultAccount" };
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre de los usuarios a excluir.")]
        public string[] Exclude
        {
            get { return ExcludeCollection; }
            set { ExcludeCollection = value; }
        }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            RAU = new UserCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                RAU.RemoveAllUsers(ExcludeDefault, ExcludeCollection);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        protected override void EndProcessing()
        {
            RAU.CloseConn();
        }
        #endregion
    }
}