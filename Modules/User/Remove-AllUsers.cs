using System.Management.Automation; //Windows PowerShell NameSpace
using System.DirectoryServices;
using System.Management;
using System.Linq;

namespace User
{
    [Cmdlet(VerbsCommon.Remove, "AllUsers")]
    public class Remove_AllUsers : Cmdlet
    {
        #region Objects
        private UserCommon RU;
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
            RU = new UserCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                RU.RemoveAllUsers(ExcludeDefault, ExcludeCollection);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        protected override void EndProcessing()
        {
            RU.CloseConn();
        }
        #endregion
    }
}