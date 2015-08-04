
namespace UAC
{
    class UACCommon
    {
        public void Set_UAC(int value)
        {
            Microsoft.Win32.Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "EnableLUA", value, Microsoft.Win32.RegistryValueKind.String);
            Microsoft.Win32.Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ConsentPromptBehaviorAdmin", value, Microsoft.Win32.RegistryValueKind.String);
        }
    }
}
