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
    [Cmdlet(VerbsCommon.Remove, "User")]
    public class PS_RemoveUser : Cmdlet
    {
        #region Objects
        private UserCommon RU;
        #endregion

        #region Parameters
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, 
            ValueFromPipelineByPropertyName = true, 
            HelpMessage = "Nombre del usuario a eliminar.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }
        #endregion

        #region Methods
        protected override void BeginProcessing()
        {
            RU = new UserCommon();
        }

        protected override void ProcessRecord()
        {
            try
            {
                RU.RemoveUser(Name);
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }

        protected override void EndProcessing()
        {
            RU.CloseConn();
        }
        #endregion
    }
}