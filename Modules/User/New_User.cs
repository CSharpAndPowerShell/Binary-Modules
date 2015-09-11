/*
CSharpAndPowerShell Modules, tries to help Microsoft Windows admins
to write automated scripts easier.
Copyright(C) 2015  Cristopher Robles Ríos

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.If not, see<http://www.gnu.org/licenses/>.

*/

using System.Management.Automation; //Windows PowerShell NameSpace

namespace User
{
    //Define el nombre del Cmdlet
    [Cmdlet(VerbsCommon.New, "User")]
    public class New_User : Cmdlet
    {
        #region Objects
        private UserCommon NU;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Nombre del nuevo usuario.")]
        [ValidateNotNullOrEmpty]
        [ValidateLength(1, 14)]
        public string Name { get; set; }

        [Parameter(Position = 1, Mandatory = false, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Contraseña para el nuevo usuario.")]
        public string Password { get; set; }

        [Parameter(Position = 2, Mandatory = false, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Descripción del nuevo usuario.")]
        public string Description { get; set; }

        [Parameter(Position = 3, Mandatory = false, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Letra en la que se montará el 'HomeDirectory'.")]
        public char HomeDirDrive { get; set; }

        [Parameter(Position = 4, Mandatory = false, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Carpeta personal del nuevo usuario.")]
        public string HomeDirectory { get; set; }

        [Parameter(Position = 5, Mandatory = false, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Script de inicio de sesión para el nuevo usuario.")]
        public string LoginScript { get; set; }

        [Parameter(Position = 6, Mandatory = false, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Ruta al perfil para el nuevo usuario.")]
        public string Profile { get; set; }

        [Parameter(Position = 7, Mandatory = false, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Grupo al que pertenecerá el nuevo usuario.")]
        public string Group { get; set; }

        [Parameter(Position = 8, Mandatory = false, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Propiedades para el nuevo usuario.")]
        public int UserFlags { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            NU = new UserCommon();
        }

        protected override void ProcessRecord()
        {
            try
            {
                NU.NewUser(Name, Password, Description,
                    HomeDirDrive, HomeDirectory, LoginScript,
                    Profile, Group, UserFlags);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        protected override void EndProcessing()
        {
            NU.CloseConn();
        }
        #endregion
    }
}