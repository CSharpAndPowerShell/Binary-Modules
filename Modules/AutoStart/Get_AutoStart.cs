using System.Management.Automation; //Windows PowerShell NameSpace

namespace AutoStart
{
    [Cmdlet(VerbsCommon.Get, "AutoStart")]
    public class Get_AutoStart : Cmdlet
    {
        #region Methods
        protected override void ProcessRecord()
        {
            try
            {
                Microsoft.Win32.RegistryKey RegKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                WriteObject(RegKey.GetValueNames());
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}