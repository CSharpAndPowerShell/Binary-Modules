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
using System.Windows.Forms;

namespace UI
{
    //Define el nombre del Cmdlet
    [Cmdlet(VerbsCommon.Show, "MessageBox")]
    public class Show_MessageBox : Cmdlet
    {
        #region Objects
        private UICommon SMB;
        #endregion

        #region Parameters
        [Parameter(Position = 0,
            HelpMessage = "Mensaje de la ventana.")]
        public string Message { get; set; }

        [Parameter(Position = 1,
            HelpMessage = "Título de la ventana.")]
        public string Title { get; set; }

        [Parameter(Position = 2,
            HelpMessage = "Icono de la ventana.")]
        [ValidateSet("Asterisk", "Error", "Exclamation", "Hand",
            "Information", "None", "Question", "Stop", "Warning")]
        public MessageBoxIcon Icon { get; set; }

        [Parameter(Position = 3,
            HelpMessage = "Botones de la ventana.")]
        [ValidateSet("AbortRetryIgnore", "OK", "OKCancel",
            "RetryCancel", "YesNo", "YesNoCancel")]
        public MessageBoxButtons Buttons { get; set; }
        #endregion

        #region Methods
        protected override void BeginProcessing()
        {
            SMB = new UICommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                WriteObject(SMB.ShowMessageBox(Message, Title, Buttons, Icon));
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}