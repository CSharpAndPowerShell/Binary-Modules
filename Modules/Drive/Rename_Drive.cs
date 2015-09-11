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

namespace Drive
{
    [Cmdlet(VerbsCommon.Rename, "Drive")]
    public class Rename_Drive : Cmdlet
    {
        #region Objects
        private DriveCommon RD;
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Letra de la unidad.")]
        [ValidateNotNullOrEmpty]
        public char Letter { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Nombre de la unidad.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }
        #endregion

        #region Methods
        protected override void BeginProcessing()
        {
            RD = new DriveCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                RD.RenameDrive(Letter, Name);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}