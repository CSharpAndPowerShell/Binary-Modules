using System.Collections;
using System.DirectoryServices;
using System.Management;

namespace User
{
    class UserCommon
    {
        #region Objects
        private DirectoryEntry AD = new DirectoryEntry("WinNT://" + System.Environment.MachineName + ",computer");
        private DirectoryEntry User;
        private DirectoryEntry Group;
        #endregion
        #region Methods
        #region Users
        public void NewUser(bool Exist, string Name, string Password, string Description, char HomeDirDrive, string HomeDirectory, string LoginScript, string Profile, int UserFlags, string Group)
        {
            //Se crea el objeto del usuario
            if (Exist)
            {
                User = AD.Children.Find(Name, "user");
            }
            else
            {
                User = AD.Children.Add(Name, "user");
            }
            //Los siguientes if validan si la variable no esta vacía, de ser así se aplican las propiedades
            if (Password != null)
            {
                //Contraseña para la nueva cuenta de usuario
                User.Invoke("SetPassword", new object[] { Password });
            }
            if (Description != null)
            {
                //Descripción de la nueva cuenta de usuario
                User.Invoke("Put", new object[] { "Description", Description });
            }
            if (HomeDirDrive != '\0') // '\0' es equivalente a null para los char
            {
                //Letra de la unidad donde se montará la carpeta particular
                User.Invoke("Put", new object[] { "HomeDirDrive", HomeDirDrive + ":" });
            }
            if (HomeDirectory != null)
            {
                //Carpeta particular del usuario
                User.Invoke("Put", new object[] { "HomeDirectory", HomeDirectory });
            }
            if (LoginScript != null)
            {
                //Script de inicio de sesión
                User.Invoke("Put", new object[] { "LoginScript", LoginScript });
            }
            if (Profile != null)
            {
                //Perfil móvil
                User.Invoke("Put", new object[] { "Profile", Profile });
            }
            if (UserFlags != 0)
            {
                //Propiedades del usuario
                User.Invoke("Put", new object[] { "UserFlags", UserFlags });
            }
            //Aplicando los cambios
            User.CommitChanges();
            //Asignar a un grupo
            if (Group != null)
            {
                AddToGroup(Name, Group);
            }
        }
        public void RemoveUser(string Name)
        {
            //Se busca que el usuario exista y se carga en el objeto
            User = AD.Children.Find(Name, "user");
            if (User != null)
            {
                //Si el usuario existe se elimina
                AD.Children.Remove(User);
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
            User = AD.Children.Find(Name, "user");
            //Se apunta al grupo
            ArrayList Groups = GetGroups();
            if (!(Groups.Contains(Grp)))
            {
                NewGroup(Grp, "");
            }
            Group = AD.Children.Find(Grp, "group");
            Group.Invoke("Add", new object[] { User.Path.ToString() });
        }
        public void NewGroup(string Name, string Description)
        {
            //Se crea el grupo
            Group = AD.Children.Add(Name, "group");
            //El siguiente if valida si la variable no esta vacía, de ser así se aplican la propiedad
            if (Description != null)
            {
                //Descripción del nuevo grupo
                Group.Invoke("Put", new object[] { "Description", Description });
            }
            //Aplicando los cambios
            Group.CommitChanges();
        }
        public void RemoveGroup(string Name)
        {
            Group = AD.Children.Find(Name);
            AD.Children.Remove(Group);
        }
        #endregion
        public void CloseConn()
        {
            //Cerrando conexiones
            if (AD != null)
            {
                //Cierra conexión a DirectoryService
                AD.Close();
            }
            if (AD != null)
            {
                //Cierra conexión a DirectoryService
                User.Close();
            }
            if (AD != null)
            {
                //Cierra conexión a DirectoryService
                Group.Close();
            }
        }
        #endregion
    }
}
