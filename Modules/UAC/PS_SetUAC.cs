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

using System;
using System.Management.Automation; //Windows PowerShell NameSpace

namespace UAC
{
    [Cmdlet(VerbsCommon.Set, "UAC")]
    public class PS_SetUAC : Cmdlet
    {
        #region Objects
        private UACCommon SUAC;
        #endregion
        #region Parameters
        [Parameter(Position = 0,
            HelpMessage = "Activa el control de cuentas de usuario.")]
        public SwitchParameter Enable
        {
            get { return enable; }
            set { enable = value; }
        }
        private bool enable;

        [Parameter(Position = 0,
            HelpMessage = "Desactiva el control de cuentas de usuario.")]
        public SwitchParameter Disable
        {
            get { return disable; }
            set { disable = value; }
        }
        private bool disable;
        #endregion

        #region Methods
        protected override void BeginProcessing()
        {
            SUAC = new UACCommon();
        }

        protected override void ProcessRecord()
        {
            try
            {
                // Validando que no se establezcan ambos modificadores
                if (!(enable && disable))
                {
                    /*
                    Se convierte el boolean a entero true = 1, false = 0
                    No se pueden establecer ambos modificadores, por lo que si se usa
                    Disable, el enable esta implícito en false
                    */
                    SUAC.Set_UAC(Convert.ToInt32(enable));
                }
                else
                {
                    // Creando error, cuando se establecen ambos modificadores
                    ErrorRecord e = new ErrorRecord(new System.Exception("Overload"),
                        "Passed more than one parameter", ErrorCategory.SyntaxError, SUAC);

                    // Mostrando error
                    WriteError(e);
                }
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}