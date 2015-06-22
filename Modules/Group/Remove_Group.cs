using System.Management.Automation; //Windows PowerShell NameSpace
using System.DirectoryServices;
namespace Group
{
    [Cmdlet(VerbsCommon.Remove, "Group")]
    public class Remove_User : Cmdlet
    {
        #region Parameters
        [Parameter(Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Nombre del grupo a eliminar.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }
        #endregion
        #region Objects
        private DirectoryEntry AD;
        private DirectoryEntry Group;
        #endregion
        protected override void BeginProcessing()
        {
            AD = new DirectoryEntry("WinNT://" + System.Environment.MachineName + ",computer");
        }
        protected override void ProcessRecord()
        {
            try
            {
                //Se elimina el usuario
                Group = AD.Children.Find(Name, "group");
                AD.Children.Remove(Group);
            }
            catch (System.Exception)
            {
                //Cerrando conexiones
                AD.Close();
                //Cerrar conexion hacia el usuario
                Group.Close();
            }
        }
        protected override void EndProcessing()
        {
            //Cerrando conexiones
            AD.Close();
            //Cerrar conexion hacia el usuario
            Group.Close();
        }
    }
}