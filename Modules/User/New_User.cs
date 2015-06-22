using System.Management.Automation; //Windows PowerShell NameSpace
using System.DirectoryServices;

namespace User
{
    //Define el nombre del Cmdlet
    [Cmdlet(VerbsCommon.New, "User")]
    public class New_User : Cmdlet
    {
        #region Parameters
        //Declaración de parámetros
        [Parameter(Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Nombre del nuevo usuario.")]
        [ValidateNotNullOrEmpty]
        [ValidateLength(1, 14)]
        public string Name { get; set; }
        [Parameter(Position = 1,
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Contraseña para el nuevo usuario.")]
        public string Password { get; set; }
        [Parameter(Position = 2,
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Descripción del nuevo usuario.")]
        public string Description { get; set; }
        [Parameter(Position = 3,
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Letra en la que se montará el 'HomeDirectory'.")]
        [ValidateLength(1, 1)]
        public string HomeDirDrive { get; set; }
        [Parameter(Position = 4,
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Carpeta personal del nuevo usuario.")]
        public string HomeDirectory { get; set; }
        [Parameter(Position = 5,
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Script de inicio de sesión para el nuevo usuario.")]
        public string LoginScript { get; set; }
        [Parameter(Position = 6,
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Ruta al perfil para el nuevo usuario.")]
        public string Profile { get; set; }
        [Parameter(Position = 7,
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Grupo al que pertenecerá el nuevo usuario.")]
        [ValidateLength(1, 14)]
        public string Group { get; set; }
        [Parameter(Position = 8,
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Propiedades para el nuevo usuario.")]
        public int UserFlags { get; set; }
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
                //Se crea el objeto del usuario
                User = AD.Children.Add(Name, "user");
                if (Password != null)
                {
                    User.Invoke("SetPassword", new object[] { Password });
                }
                if (Description != null)
                {
                    User.Invoke("Put", new object[] { "Description", Description });
                }
                if (HomeDirDrive != null)
                {
                    User.Invoke("Put", new object[] { "HomeDirDrive", HomeDirDrive + ":" });
                }
                if (HomeDirectory != null)
                {
                    User.Invoke("Put", new object[] { "HomeDirectory", HomeDirectory });
                }
                if (LoginScript != null)
                {
                    User.Invoke("Put", new object[] { "LoginScript", LoginScript });
                }
                if (Profile != null)
                {
                    User.Invoke("Put", new object[] { "Profile", Profile });
                }
                if (UserFlags != null)
                {
                    User.Invoke("Put", new object[] { "UserFlags", UserFlags });
                }
                //Se aplican los cambios
                User.CommitChanges();
                //Se añade a un grupo
                Grp = AD.Children.Find(Group, "group");
                if (Grp != null)
                {
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
    }
}