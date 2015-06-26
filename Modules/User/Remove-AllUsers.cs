using System.Management.Automation; //Windows PowerShell NameSpace
using System.DirectoryServices;
using System.Management;
using System.Linq;

namespace Group
{
    [Cmdlet(VerbsCommon.Remove, "AllUsers")]
    public class Remove_AllUsers : Cmdlet
    {
        #region Objects
        private UserCommon RU;
        private string[] ExcludeCollection = { "" };
        private string[] ExcludeDefault = { "Administrador", "uno", "Invitado", "DefaultAccount" };
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre de los usuarios a excluir.")]
        public string[] Exclude
        {
            get { return ExcludeCollection; }
            set { ExcludeCollection = value; }
        }
        [Parameter(Position = 1, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del usuario a eliminar.")]
        public bool All { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            RU = new UserCommon();
        }
        protected override void ProcessRecord()
        {
            SelectQuery Query = new SelectQuery("Win32_UserAccount");
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Query);
            foreach (ManagementObject User in Searcher.Get())
            {
                if (All)
                {
                    if ((!(ExcludeCollection.Contains(User["Name"].ToString()))))
                    {
                        RU.RemoveUser(User["Name"].ToString());
                    }
                }
                else if ((!(ExcludeCollection.Contains(User["Name"].ToString()))) && (!(ExcludeDefault.Contains(User["Name"].ToString()))))
                {
                    RU.RemoveUser(User["Name"].ToString());
                }
            }
        }
        protected override void EndProcessing()
        {
            RU.CloseConn(true, true, false);
        }
        #endregion
    }
}