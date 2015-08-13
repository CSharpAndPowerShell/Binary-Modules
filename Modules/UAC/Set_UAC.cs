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

using System.Management.Automation; //Windows PowerShell NameSpace

namespace UAC
{
    [Cmdlet(VerbsCommon.Set, "UAC")]
    public class Set_UAC : Cmdlet
    {
        #region Objects
        private UACCommon SUAC;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = false, HelpMessage = "Valor boleano de la propiedad.")]
        public SwitchParameter Enable
        {
            get { return enable; }
            set { enable = value; }
        }
        private bool enable;
        [Parameter(Position = 0, Mandatory = false, HelpMessage = "Valor boleano de la propiedad.")]
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
                if (!(enable && disable))
                {
                    int value = 0;
                    if (enable)
                    {
                        value = 1;
                    }
                    if (disable)
                    {
                        value = 0;
                    }
                    SUAC.Set_UAC(value);
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