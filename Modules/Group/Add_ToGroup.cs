using System.Management.Automation; //Windows PowerShell NameSpace
using System.DirectoryServices;
namespace Group
{
    [Cmdlet(VerbsCommon.Add, "ToGroup")]
    public class Add_ToGroup : Cmdlet
    {
        #region Parameters
        [Parameter(Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Nombre del nuevo usuario.")]
        [ValidateNotNullOrEmpty]
        [ValidateLength(1, 14)]
        public string Name { get; set; }
        [Parameter(Position = 1,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Grupo al que pertenecerá el nuevo usuario.")]
        [ValidateNotNullOrEmpty]
        [ValidateLength(1, 14)]
        public string Group { get; set; }
        #endregion
        #region Objects
        private DirectoryEntry AD;
        private DirectoryEntry User;
        private DirectoryEntry Grp;
        #endregion
        protected override void BeginProcessing()
        {
            AD = new DirectoryEntry("WinNT://" + System.Environment.MachineName + ",computer");
        }
        protected override void ProcessRecord()
        {
            try
            {
                //Se apunta al usuario
                User = AD.Children.Find(Name, "user");
                //Se apunta al grupo
                Grp = AD.Children.Find(Group, "group");
                if (Grp != null && User != null)
                {
                    Grp.Invoke("Add", new object[] { User.Path.ToString() });
                }
                else
                {
                    System.Console.Write("Nombre de usuario o grupo no válido");
                }
            }
            catch (System.Exception)
            {
                //Cerrando conexiones
                AD.Close();
                //Cerrar conexion hacia el usuario
                User.Close();
            }
        }
        protected override void EndProcessing()
        {
            //Cerrando conexiones
            AD.Close();
            //Cerrar conexion hacia el usuario
            User.Close();
        }
    }
}