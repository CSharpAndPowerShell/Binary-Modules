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

using System.Windows.Forms;

namespace UI
{
    public class UICommon
    {
        public string ShowMessageBox(string Message, string Title, string Buttons, string Icon)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            switch (Buttons)
            {
                case "AbortRetryIgnore":
                    buttons = MessageBoxButtons.AbortRetryIgnore;
                    break;
                case "OK":
                    buttons = MessageBoxButtons.OK;
                    break;
                case "OKCancel":
                    buttons = MessageBoxButtons.OKCancel;
                    break;
                case "RetryCancel":
                    buttons = MessageBoxButtons.RetryCancel;
                    break;
                case "YesNo":
                    buttons = MessageBoxButtons.YesNo;
                    break;
                case "YesNoCancel":
                    buttons = MessageBoxButtons.YesNoCancel;
                    break;
                default:
                    buttons = MessageBoxButtons.OK;
                    break;
            }
            MessageBoxIcon icon = MessageBoxIcon.None;
            switch (Icon)
            {
                case "Asterisk":
                    icon = MessageBoxIcon.Asterisk;
                    break;
                case "Error":
                    icon = MessageBoxIcon.Error;
                    break;
                case "Exclamation":
                    icon = MessageBoxIcon.Exclamation;
                    break;
                case "Hand":
                    icon = MessageBoxIcon.Hand;
                    break;
                case "Information":
                    icon = MessageBoxIcon.Information;
                    break;
                case "None":
                    icon = MessageBoxIcon.None;
                    break;
                case "Question":
                    icon = MessageBoxIcon.Question;
                    break;
                case "Stop":
                    icon = MessageBoxIcon.Stop;
                    break;
                case "Warning":
                    icon = MessageBoxIcon.Warning;
                    break;
                default:
                    icon = MessageBoxIcon.None;
                    break;
            }
            switch (MessageBox.Show(Message, Title, buttons, icon))
            {
                case DialogResult.Abort:
                    return "Abort";
                case DialogResult.Cancel:
                    return "Cancel";
                case DialogResult.Ignore:
                    return "Ignore";
                case DialogResult.No:
                    return "No";
                case DialogResult.None:
                    return null;
                case DialogResult.OK:
                    return "OK";
                case DialogResult.Retry:
                    return "Retry";
                case DialogResult.Yes:
                    return "Yes";
                default:
                    return null;
            }
        }
    }
}
