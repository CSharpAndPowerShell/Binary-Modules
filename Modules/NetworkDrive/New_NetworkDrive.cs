﻿using System.Management.Automation; //Windows PowerShell NameSpace

namespace NetworkDrive
{
    [Cmdlet(VerbsCommon.New, "NetworkDrive")]
    public class New_NetworkDrive : Cmdlet
    {
        #region Objects
        private NetworkDriveCommon NND;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Letra a montar.")]
        [ValidateNotNullOrEmpty]
        public char Letter { get; set; }
        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Ruta UNC del recurso compartido.")]
        [ValidateNotNullOrEmpty]
        public string Path { get; set; }
        [Parameter(Position = 2, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre de usuario que tiene acceso al recurso.")]
        public string User { get; set; }
        [Parameter(Position = 3, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Contraseña del usuario.")]
        public string Password { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            NND = new NetworkDriveCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                NND.NewNetworkDrive(Letter, Path, User, Password);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}