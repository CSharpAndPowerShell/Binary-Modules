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

namespace Share
{
    [Cmdlet(VerbsCommon.Remove, "AllShares")]
    public class Remove_AllShares : Cmdlet
    {
        #region Objects
        private ShareCommon RAS;
        private string[] ExcludeCollection;
        private string[] ExcludeDefault = { "ADMIN$", "C$", "IPC$" };
        #endregion
        #region Parameters
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Nombre de los usuarios a excluir.")]
        public string[] Exclude
        {
            get { return ExcludeCollection; }
            set { ExcludeCollection = value; }
        }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            RAS = new ShareCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                RAS.RemoveAllShares(ExcludeDefault, ExcludeCollection);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}
