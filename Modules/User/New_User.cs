using System.Management.Automation; //Windows PowerShell NameSpace
using System.DirectoryServices;

namespace User
{
    //Define el nombre del Cmdlet
    [Cmdlet(VerbsCommon.New, "User")]
    public class New_User : Cmdlet
    {
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre del nuevo usuario.")]
        [ValidateNotNullOrEmpty]
        [ValidateLength(1, 14)]
        public string Name { get; set; }
        [Parameter(Position = 1, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Contraseña para el nuevo usuario.")]
        public string Password { get; set; }
        [Parameter(Position = 2, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Descripción del nuevo usuario.")]
        public string Description { get; set; }
        [Parameter(Position = 3, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Letra en la que se montará el 'HomeDirectory'.")]
        [ValidateLength(1, 1)]
        public string HomeDirDrive { get; set; }
        [Parameter(Position = 4, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Carpeta personal del nuevo usuario.")]
        public string HomeDirectory { get; set; }
        [Parameter(Position = 5, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Script de inicio de sesión para el nuevo usuario.")]
        public string LoginScript { get; set; }
        [Parameter(Position = 6, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Ruta al perfil para el nuevo usuario.")]
        public string Profile { get; set; }
        [Parameter(Position = 7, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Grupo al que pertenecerá el nuevo usuario.")]
        public string Group { get; set; }
        [Parameter(Position = 8, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Propiedades para el nuevo usuario.")]
        public int UserFlags { get; set; }
        #endregion
        #region Objects
        private DirectoryEntry AD;
        private DirectoryEntry User;
        private DirectoryEntry Grp;
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
                //Se crea el objeto del usuario
                User = AD.Children.Add(Name, "user");
                //Los siguientes if validan si la variable no esta vacía, de ser así se aplican las propiedades
                if (Password != null)
                {
                    //Contraseña para la nueva cuenta de usuario
                    User.Invoke("SetPassword", new object[] { Password });
                }
                if (Description != null)
                {
                    //Descripción de la nueva cuenta de usuario
                    User.Invoke("Put", new object[] { "Description", Description });
                }
                if (HomeDirDrive != null)
                {
                    //Letra de la unidad donde se montará la carpeta particular
                    User.Invoke("Put", new object[] { "HomeDirDrive", HomeDirDrive + ":" });
                }
                if (HomeDirectory != null)
                {
                    //Carpeta particular del usuario
                    User.Invoke("Put", new object[] { "HomeDirectory", HomeDirectory });
                }
                if (LoginScript != null)
                {
                    //Script de inicio de sesión
                    User.Invoke("Put", new object[] { "LoginScript", LoginScript });
                }
                if (Profile != null)
                {
                    //Perfil móvil
                    User.Invoke("Put", new object[] { "Profile", Profile });
                }
                if (UserFlags != 0)
                {
                    //Propiedades del usuario
                    User.Invoke("Put", new object[] { "UserFlags", UserFlags });
                }
                //Aplicando los cambios
                User.CommitChanges();
                //Asignar a un grupo
                //Obtiene el grupo
                Grp = AD.Children.Find(Group, "group");
                if (Grp != null)
                {
                    //Se agrega el usuario al grupo
                    Grp.Invoke("Add", new object[] { User.Path.ToString() });
                }
                else if ((Group != null) && (Grp == null)) {
                    //Si el grupo no existe se crea
                    Grp = AD.Children.Add(Name, "group");
                    //Se aplican los cambios
                    Grp.CommitChanges();
                    //Obtiene el grupo
                    Grp = AD.Children.Find(Group, "group");
                    //Se agrega el usuario al grupo
                    Grp.Invoke("Add", new object[] { User.Path.ToString() });
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