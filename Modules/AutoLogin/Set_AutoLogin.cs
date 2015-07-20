using System.Management.Automation; //Windows PowerShell NameSpace

namespace AutoLogin
{
    [Cmdlet(VerbsCommon.Set, "AutoLogin")]
    public class Set_AutoLogin : Cmdlet
    {
        #region Objects
        private AutoLoginCommon SAL;
        #endregion
        #region Parameters
        [Parameter(Position = 0, HelpMessage = "Deshabilitar el Autologin.")]
        public SwitchParameter Disable
        {
            get { return disable; }
            set { disable = value; }
        }
        private bool disable;
        [Parameter(Position = 0, HelpMessage = "Nombre del usuario a iniciar sesión automáticamente.")]
        [ValidateNotNullOrEmpty]
        public string User { get; set; }
        [Parameter(Position = 1, HelpMessage = "Contraseña del usuario a iniciar sesión automáticamente.")]
        public string Password { get; set; }
        #endregion
        #region Methods
        protected override void BeginProcessing()
        {
            SAL = new AutoLoginCommon();
        }
        protected override void ProcessRecord()
        {
            try
            {
                if (disable)
                {
                    SAL.SetAutoLogin(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "AutoAdminLogon", "0");
                }
                else
                {
                    SAL.SetAutoLogin(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "AutoAdminLogon", "1");
                    SAL.SetAutoLogin(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "DefaultUsername", User);
                    if (Password == null)
                    {
                        Password = "";
                    }
                    SAL.SetAutoLogin(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "DefaultPassword", Password);
                }
            }
            catch (PSInvalidOperationException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
        #endregion
    }
}