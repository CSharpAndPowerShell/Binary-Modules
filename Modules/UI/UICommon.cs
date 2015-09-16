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
        public string ShowMessageBox(string message, string title, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            return MessageBox.Show(message, title, buttons, icon).ToString();
        }
    }
}
