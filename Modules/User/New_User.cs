using System.Management.Automation; //Windows PowerShell NameSpace

namespace Group
{
    //Define el nombre del Cmdlet
    [Cmdlet(VerbsCommon.New, "User")]
    public class New_User : Cmdlet
    {
        #region Objects
        private UserCommon NU;
        #endregion
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
        public char HomeDirDrive { get; set; }
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
        #region Methods
        protected override void BeginProcessing()
        {
            NU = new UserCommon();
        }
        protected override void ProcessRecord()
        {
            NU.NewUser(Name,Password,Description, HomeDirDrive, HomeDirectory, LoginScript, Profile, UserFlags, Group);
        }
        protected override void EndProcessing()
        {
            NU.CloseConn(true, true, true);
        }
        #endregion
    }
}