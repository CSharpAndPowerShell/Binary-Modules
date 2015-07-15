
namespace UAC
{
    class UACCommon
    {
        public void Set_UAC(string key, string name, int value)
        {
            Microsoft.Win32.Registry.SetValue(key, name, value, Microsoft.Win32.RegistryValueKind.String);
        }
    }
}
