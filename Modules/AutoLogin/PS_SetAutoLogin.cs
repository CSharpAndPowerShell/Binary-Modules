/*
CSharpAndPowerShell Modules, tries to help Microsoft Windows admins to write automated scripts easier.
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

using System;
using System.Management.Automation; //Windows PowerShell NameSpace

namespace AutoLogin
{
    [Cmdlet(VerbsCommon.Set, "AutoLogin")]
    public class PS_SetAutoLogin : Cmdlet
    {
        #region Objects
        private AutoLoginCommon SAL;
        #endregion

        #region Parameters
        [Parameter(Position = 0,
            HelpMessage = "Deshabilitar el Autologin.")]
        public SwitchParameter Disable
        {
            get { return disable; }
            set { disable = value; }
        }
        private bool disable;

        [Parameter(Position = 0,
            HelpMessage = "Nombre del usuario a iniciar sesión automáticamente.")]
        [ValidateNotNullOrEmpty]
        public string User { get; set; }

        [Parameter(Position = 1,
            HelpMessage = "Contraseña del usuario a iniciar sesión automáticamente.")]
        [ValidateNotNull]
        public string Password { get; set; }
        #endregion

        #region Methods
        protected override void BeginProcessing()
        {
            SAL = new AutoLoginCommon();
        }

        protected override void ProcessRecord()
        {
            try
            {
                SAL.SetAutoLogin("AutoAdminLogon", Convert.ToInt32(!disable).ToString());
                SAL.SetAutoLogin("DefaultUsername", User);
                SAL.SetAutoLogin("DefaultPassword", Password);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}