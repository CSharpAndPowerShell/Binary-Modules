using System.Management.Automation; //Windows PowerShell NameSpace
using System.DirectoryServices;

namespace User
{
    [Cmdlet(VerbsCommon.Remove, "User")]
    public class Remove_User : Cmdlet
    {
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del usuario a eliminar.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }
        #endregion
        #region Objects
        private DirectoryEntry AD;
        private DirectoryEntry User;
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
                //Se busca que el usuario exista y se carga en el objeto
                User = AD.Children.Find(Name, "user");
                if (User != null) {
                    //Si el usuario existe se elimina
                    AD.Children.Remove(User);
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
        #endregion
    }
}