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
    [Cmdlet(VerbsCommon.New, "RestorePoint")]
    public class PS_NewRestorePoint : Cmdlet
    {
        #region Objects
        private RestorePointCommon NRPC;
        #endregion

        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Descripción del punto de restauración.")]
        [ValidateNotNullOrEmpty]
        public string Description { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Tipo del punto de restauración.")]
        [ValidateSet("APPLICATION_INSTALL", "APPLICATION_UNINSTALL",
            "CANCELLED_OPERATION", "DEVICE_DRIVER_INSTALL", "MODIFY_SETTINGS")]
        [ValidateNotNullOrEmpty]
        public RestorePointCommon.RestorePointType RestorePointType { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Tipo de evento del punto de restauración.")]
        [ValidateSet("BEGIN_SYSTEM_CHANGE", "END_SYSTEM_CHANGE",
            "BEGIN_NESTED_SYSTEM_CHANGE", "END_NESTED_SYSTEM_CHANGE")]
        [ValidateNotNullOrEmpty]
        public RestorePointCommon.EventType EventType { get; set; }
        #endregion

        #region Methods
        protected override void BeginProcessing()
        {
            NRPC = new RestorePointCommon();
        }

        protected override void ProcessRecord()
        {
            try
            {
                NRPC.NewRestorePoint(Description, RestorePointType, EventType);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}
