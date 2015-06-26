using System.Management.Automation; //Windows PowerShell NameSpace
using System.DirectoryServices;

namespace Group
{
    [Cmdlet(VerbsCommon.New, "Group")]
    public class New_User : Cmdlet
    {
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del nuevo grupo.")]
        [ValidateNotNullOrEmpty]
        [ValidateLength(1, 14)]
        public string Name { get; set; }
        [Parameter(Position = 1, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Descripción del nuevo grupo.")]
        public string Description { get; set; }
        #endregion
        #region Objects
        private DirectoryEntry AD;
        private DirectoryEntry Group;
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
                //Se crea el grupo
                Group = AD.Children.Add(Name, "group");
                //El siguiente if valida si la variable no esta vacía, de ser así se aplican la propiedad
                if (Description != null)
                {
                    //Descripción del nuevo grupo
                    Group.Invoke("Put", new object[] { "Description", Description });
                }
                //Aplicando los cambios
                Group.CommitChanges();
            }
            catch (System.Exception)
            {
                //Cerrando conexiones
                AD.Close();
                //Cerrar conexion hacia el grupo
                Group.Close();
            }
        }
        protected override void EndProcessing()
        {
            //Cerrando conexiones
            AD.Close();
            //Cerrar conexion hacia el grupo
            Group.Close();
        }
        #endregion
    }
}