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

using System.Collections;
using System.DirectoryServices;
using System.Management;

namespace User
{
    public class UserCommon
    {
        #region Objects
        private DirectoryEntry AD =
            new DirectoryEntry("WinNT://" + System.Environment.MachineName + ",computer");
        private DirectoryEntry DEUser;
        private DirectoryEntry DEGroup;
        #endregion

        #region Methods
        #region Users
        public void NewUser(string Name, string Password, string Description,
            char HomeDirDrive, string HomeDirectory, string LoginScript,
            string Profile, string Group, int UserFlags)
        {
            // Verifica si el usuario existe
            if (isUser(Name))
            {
                // Si existe se referencia
                DEUser = AD.Children.Find(Name, "user");
            }
            else
            {
                // Si no se agrega uno nuevo
                DEUser = AD.Children.Add(Name, "user");
            }
            // Los siguientes if validan si la variable no esta vacía, de ser así se aplican las propiedades
            if (Password != null)
            {
                // Contraseña para la nueva cuenta de usuario
                DEUser.Invoke("SetPassword", new object[] { Password });
            }
            if (Description != null)
            {
                // Descripción de la nueva cuenta de usuario
                DEUser.Invoke("Put", new object[] { "Description", Description });
            }
            if (HomeDirDrive != '\0') // '\0' es equivalente a null para los char
            {
                // Letra de la unidad donde se montará la carpeta particular
                DEUser.Invoke("Put", new object[] { "HomeDirDrive", HomeDirDrive + ":" });
            }
            if (HomeDirectory != null)
            {
                // Carpeta particular del usuario
                DEUser.Invoke("Put", new object[] { "HomeDirectory", HomeDirectory });
            }
            if (LoginScript != null)
            {
                // Script de inicio de sesión
                DEUser.Invoke("Put", new object[] { "LoginScript", LoginScript });
            }
            if (Profile != null)
            {
                // Perfil móvil
                DEUser.Invoke("Put", new object[] { "Profile", Profile });
            }
            if (UserFlags != 0)
            {
                // Propiedades del usuario
                DEUser.Invoke("Put", new object[] { "UserFlags", UserFlags });
            }
            // Aplicando los cambios
            DEUser.CommitChanges();
            // Agregar a un grupo
            if (Group != null)
            {
                AddToGroup(Name, Group);
            }
        }

        public void RemoveUser(string Name)
        {
            // Se referencia al usuario
            DEUser = AD.Children.Find(Name, "user");
            // Se elimina el usuario
            AD.Children.Remove(DEUser);
        }

        private ArrayList GetUsers(string[] Default = null, string[] Custom = null)
        {
            // Devuelve la lista de los nombres de usuarios
            // Se hace la consulta y se obtienen los usuarios
            SelectQuery Query = new SelectQuery("Win32_UserAccount");

            // Se crea un objeto buscador
            ManagementObjectSearcher Searcher =
                new ManagementObjectSearcher(Query);

            // Lista para almacenar los nombres de los usuarios
            ArrayList Users = new ArrayList();

            // Se agregan los nombres al ArrayList
            foreach (ManagementObject User in Searcher.Get())
            {
                Users.Add(User["Name"].ToString());
            }

            // Se aplican los filtros
            if (Default != null)
            {
                foreach (var item in Default)
                {
                    Users.Remove(item);
                }
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

        private bool isUser(string User)
        {
            // Verifica si el Usuario existe
            ArrayList Users = GetUsers();
            return Users.Contains(User);
        }

        public void RemoveAllUsers(string[] Default, string[] Custom)
        {
            // Elimina todos los usuarios y se excluyen los que se pasen por los parametros "Default" y "Custom"
            // Se obtiene lista de usuarios filtrada
            ArrayList Users = GetUsers(Default, Custom);
            // Se eliminan los usuarios
            foreach (string User in Users)
            {
                RemoveUser(User);
            }
        }
        #endregion

        #region Groups
        private ArrayList GetGroups()
        {
            // Devuelve la lista de los nombres de los grupos
            // Se hace la consulta y se obtienen los grupos
            SelectQuery Query = new SelectQuery("Win32_Group");
            // Se crea un objeto buscador
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Query);
            // Objeto donde se almacenará la lista de grupos
            ArrayList Groups = new ArrayList();
            // Se agregan los nombres al ArrayList
            foreach (ManagementObject User in Searcher.Get())
            {
                Groups.Add(User["Name"].ToString());
            }
            return Groups;
        }

        private bool isGroup(string Group)
        {
            // Verifica si el Group existe
            ArrayList Groups = GetGroups();
            return Groups.Contains(Group);
        }

        public void AddToGroup(string Name, string Grp)
        {
            // Agrega un usuario a un grupo

            // Se apunta al usuario
            DEUser = AD.Children.Find(Name, "user");

            // De no existir el grupo se crea
            if (!isGroup(Grp))
            {
                NewGroup(Grp, "");
            }

            // Se apunta al grupo
            DEGroup = AD.Children.Find(Grp, "group");

            // Se agrega el usuario al grupo
            DEGroup.Invoke("Add", new object[] { DEUser.Path.ToString() });
        }

        public void NewGroup(string Name, string Description)
        {
            // Crea un nuevo grupo

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
            // Se referencia al grupo
            DEGroup = AD.Children.Find(Name);
            // Se elimina el grupo
            AD.Children.Remove(DEGroup);
        }
        #endregion

        public void CloseConn()
        {
            //Cerrando conexiones a DirectoryService
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
