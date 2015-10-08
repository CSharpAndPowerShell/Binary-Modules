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

using System.Management.Automation;

namespace RestorePoint
{
    [Cmdlet(VerbsCommon.Set, "RestorePoint")]
    public class PS_SetRestorePoint : Cmdlet
    {
        #region Objects
        private RestorePointCommon SRPC;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Habilita la restauración en la unidad.")]
        public SwitchParameter Enable
        {
            get { return enable; }
            set { enable = value; }
        }
        private bool enable;

        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Deshabilita la restauración en la unidad.")]
        public SwitchParameter Disable
        {
            get { return disable; }
            set { disable = value; }
        }
        private bool disable;

        [Parameter(Position = 1, Mandatory = false, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Unidad a activar o desactivar la restauración.")]
        [ValidateNotNullOrEmpty]
        public string Drive { get; set; }
        #endregion

        #region Methods
        protected override void BeginProcessing()
        {
            SRPC = new RestorePointCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                if (!(enable && disable))
                {
                    if (enable)
                    {
                        SRPC.SetRestorePoint("Enable", Drive);
                    }
                    else if (disable)
                    {
                        SRPC.SetRestorePoint("Disable", Drive);
                    }
                }
                else
                {
                    // Creando error
                    ErrorRecord e = new ErrorRecord(new System.Exception("Overload"),
                        "Passed more than one parameter", ErrorCategory.SyntaxError, SRPC);

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
