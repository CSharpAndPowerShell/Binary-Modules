using System.DirectoryServices;

namespace Group
{
    class UserCommon
    {
        #region Objects
        private DirectoryEntry AD = new DirectoryEntry("WinNT://" + System.Environment.MachineName + ",computer");
        private DirectoryEntry User;
        private DirectoryEntry Grp;
        #endregion
        public void NewUser(string Name, string Password, string Description, string HomeDirDrive, string HomeDirectory, string LoginScript, string Profile, int UserFlags, string Group)
        {
            try
            {
                //Se crea el objeto del usuario
                User = AD.Children.Add(Name, "user");
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
                if (HomeDirDrive != null)
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
                //Obtiene el grupo
                Grp = AD.Children.Find(Group, "group");
                if (Grp != null)
                {
                    //Se agrega el usuario al grupo
                    Grp.Invoke("Add", new object[] { User.Path.ToString() });
                }
                else if (Group != null)
                {
                    //Si el grupo no existe se crea
                    Grp = AD.Children.Add(Name, "group");
                    //Se aplican los cambios
                    Grp.CommitChanges();
                    //Obtiene el grupo
                    Grp = AD.Children.Find(Group, "group");
                    //Se agrega el usuario al grupo
                    Grp.Invoke("Add", new object[] { User.Path.ToString() });
                }
            }
            catch
            {
                CloseConn(true, true, true);
            }
        }
        public void RemoveUser(string Name)
        {
            try
            {
                //Se busca que el usuario exista y se carga en el objeto
                User = AD.Children.Find(Name, "user");
                if (User != null)
                {
                    //Si el usuario existe se elimina
                    AD.Children.Remove(User);
                }
            }
            catch
            {
                CloseConn(true, true, false);
            }
        }
        public void CloseConn(bool _AD, bool _User, bool _Group)
        {
            //Cerrando conexiones
            if (_AD)
            {
                //Cierra conexión a DirectoryService
                AD.Close();
            }
            if (_User)
            {
                //Cerrar conexion hacia el usuario
                User.Close();
            }
            if (_Group)
            {
                //Cerrar conexion hacia el grupo
                Grp.Close();
            }
        }
    }
}
