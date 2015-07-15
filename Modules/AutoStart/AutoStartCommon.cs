
namespace AutoStart
{
    class AutoStartCommon
    {
        public void WriteReg(string key, string name, string value)
        {
            Microsoft.Win32.Registry.SetValue(key, name, value, Microsoft.Win32.RegistryValueKind.String);
        }
        public void DeleteReg(string key, string name)
        {
            Microsoft.Win32.RegistryKey RegKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(key, true);
            RegKey.DeleteValue(name);
        }
    }
}