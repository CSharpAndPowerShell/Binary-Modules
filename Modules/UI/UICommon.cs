using System.Windows.Forms;

namespace UI
{
    public class UICommon
    {
        public void ShowMessageBox(string Message, string Title, string Buttons, string Icon)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            switch (Buttons) {
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
            MessageBox.Show(Message, Title, buttons, icon);
        }
    }
}
