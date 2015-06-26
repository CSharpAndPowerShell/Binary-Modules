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
        private DirectoryEntry AD;
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
            //Abre conexión al DirectoryService
            AD = new DirectoryEntry("WinNT://" + System.Environment.MachineName + ",computer");
        }
        protected override void ProcessRecord()
        {
            try
            {
                SelectQuery query = new SelectQuery("Win32_UserAccount");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                foreach (ManagementObject user in searcher.Get())
                {
                    if ((!(ExcludeCollection.Contains(user["Name"]))) && (!(ExcludeDefault.Contains(user["Name"]))))
                    {
                        AD.Children.Remove(AD.Children.Find(user.ToString(), "user"));
                    }
                }
            }
            catch (System.Exception)
            {
                //Cerrando conexiones
                AD.Close();
            }
        }
        protected override void EndProcessing()
        {
            //Cerrando conexiones
            AD.Close();
        }
        #endregion
    }
}