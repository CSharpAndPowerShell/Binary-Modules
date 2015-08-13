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

namespace Drive
{
    [Cmdlet(VerbsCommon.Set, "Drives")]
    public class Set_Drives : Cmdlet
    {
        #region Objects
        private DriveCommon SD;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Valor boleano de la propiedad.")]
        [ValidateNotNullOrEmpty]
        public SwitchParameter NoDrives
        {
            get { return nodrives; }
            set { nodrives = value; }
        }
        private bool nodrives;
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Valor boleano de la propiedad.")]
        [ValidateNotNullOrEmpty]
        public SwitchParameter NoViewOnDrive
        {
            get { return noviewondrive; }
            set { noviewondrive = value; }
        }
        private bool noviewondrive;
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Valor boleano de la propiedad.")]
        [ValidateNotNullOrEmpty]
        public SwitchParameter Disable
        {
            get { return disable; }
            set { disable = value; }
        }
        private bool disable;
        [Parameter(Position = 1, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre de los usuarios a excluir.")]
        public string[] Drives
        {
            get { return drives; }
            set { drives = value; }
        }
        private string[] drives;
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            SD = new DriveCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                SD.SetDrives(drives, nodrives, noviewondrive, disable);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}