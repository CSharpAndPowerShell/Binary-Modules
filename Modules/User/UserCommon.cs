using System.Collections;
using System.DirectoryServices;
using System.Management;

namespace User
{
    class UserCommon
    {
        #region Objects
        private DirectoryEntry AD = new DirectoryEntry("WinNT://" + System.Environment.MachineName + ",computer");
        private DirectoryEntry DEUser;
        private DirectoryEntry DEGroup;
        #endregion
        #region Methods
        #region Users
        public void NewUser(bool Exist, string Name, string Password, string Description, char HomeDirDrive, string HomeDirectory, string LoginScript, string Profile, int UserFlags, string Group)
        {
            //Se crea el objeto del usuario
            if (Exist)
            {
                DEUser = AD.Children.Find(Name, "user");
            }
            else
            {
                DEUser = AD.Children.Add(Name, "user");
            }
            //Los siguientes if validan si la variable no esta vacía, de ser así se aplican las propiedades
            if (Password != null)
            {
                //Contraseña para la nueva cuenta de usuario
                DEUser.Invoke("SetPassword", new object[] { Password });
            }
            if (Description != null)
            {
                //Descripción de la nueva cuenta de usuario
                DEUser.Invoke("Put", new object[] { "Description", Description });
            }
            if (HomeDirDrive != '\0') // '\0' es equivalente a null para los char
            {
                //Letra de la unidad donde se montará la carpeta particular
                DEUser.Invoke("Put", new object[] { "HomeDirDrive", HomeDirDrive + ":" });
            }
            if (HomeDirectory != null)
            {
                //Carpeta particular del usuario
                DEUser.Invoke("Put", new object[] { "HomeDirectory", HomeDirectory });
            }
            if (LoginScript != null)
            {
                //Script de inicio de sesión
                DEUser.Invoke("Put", new object[] { "LoginScript", LoginScript });
            }
            if (Profile != null)
            {
                //Perfil móvil
                DEUser.Invoke("Put", new object[] { "Profile", Profile });
            }
            if (UserFlags != 0)
            {
                //Propiedades del usuario
                DEUser.Invoke("Put", new object[] { "UserFlags", UserFlags });
            }
            //Aplicando los cambios
            DEUser.CommitChanges();
            //Asignar a un grupo
            if (Group != null)
            {
                AddToGroup(Name, Group);
            }
        }
        public void RemoveUser(string Name)
        {
            //Se busca que el usuario exista y se carga en el objeto
            DEUser = AD.Children.Find(Name, "user");
            if (DEUser != null)
            {
                //Si el usuario existe se elimina
                AD.Children.Remove(DEUser);
            }
        }
        private ArrayList GetUsers(string[] Default = null, string[] Custom = null)
        {
            SelectQuery Query = new SelectQuery("Win32_UserAccount");
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Query);
            ArrayList Users = new ArrayList();
            foreach (ManagementObject User in Searcher.Get())
            {
                Users.Add(User["Name"].ToString());
            }
            foreach (var item in Default)
            {
                Users.Remove(item);
            }
            if (Custom != null)
            {
                foreach (var item in Custom)
                {
                    Users.Remove(item);
                }
            }
            return Users;
        }
        public void RemoveAllUsers(string[] Default, string[] Custom)
        {
            ArrayList Users = GetUsers(Default, Custom);
            foreach (string User in Users)
            {
                RemoveUser(User);
            }
        }
        #endregion
        #region Groups
        public ArrayList GetGroups()
        {
            SelectQuery Query = new SelectQuery("Win32_Group");
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Query);
            ArrayList Groups = new ArrayList();
            foreach (ManagementObject User in Searcher.Get())
            {
                Groups.Add(User["Name"].ToString());
            }
            return Groups;
        }
        public void AddToGroup(string Name, string Grp)
        {
            //Se apunta al usuario
            DEUser = AD.Children.Find(Name, "user");
            //Se apunta al grupo
            ArrayList Groups = GetGroups();
            if (!(Groups.Contains(Grp)))
            {
                NewGroup(Grp, "");
            }
            DEGroup = AD.Children.Find(Grp, "group");
            DEGroup.Invoke("Add", new object[] { DEUser.Path.ToString() });
        }
        public void NewGroup(string Name, string Description)
        {
            //Se crea el grupo
            DEGroup = AD.Children.Add(Name, "group");
            //El siguiente if valida si la variable no esta vacía, de ser así se aplican la propiedad
            if (Description != null)
            {
                //Descripción del nuevo grupo
                DEGroup.Invoke("Put", new object[] { "Description", Description });
            }
            //Aplicando los cambios
            DEGroup.CommitChanges();
        }
        public void RemoveGroup(string Name)
        {
            DEGroup = AD.Children.Find(Name);
            AD.Children.Remove(DEGroup);
        }
        #endregion
        public void CloseConn()
        {
            //Cerrando conexiones  a DirectoryService
            if (AD != null)
            {
                AD.Close();
            }
            if (DEUser != null)
            {
                DEUser.Close();
            }
            if (DEGroup != null)
            {
                DEGroup.Close();
            }
        }
        #endregion
    }
}
