
namespace AutoLogin
{
    public class AutoLoginCommon
    {
        public void SetAutoLogin(string key, string name, string value)
        {
            Microsoft.Win32.Registry.SetValue(key, name, value, Microsoft.Win32.RegistryValueKind.String);
        }
    }
}
