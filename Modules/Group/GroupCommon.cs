using System.DirectoryServices;

namespace User
{
    class GroupCommon
    {
        #region Objects
        private DirectoryEntry AD = new DirectoryEntry("WinNT://" + System.Environment.MachineName + ",computer");
        private DirectoryEntry User;
        private DirectoryEntry Group;
        #endregion
        #region Methods
        public void NewGroup(string Name, string Description)
        {
            try
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
            catch
            {
                CloseConn();
            }
        }
        public void AddToGroup(string Name, string Grp)
        {
            //Se apunta al usuario
            User = AD.Children.Find(Name, "user");
            //Se apunta al grupo
            Group = AD.Children.Find(Grp, "group");
            if (Group != null && User != null)
            {
                Group.Invoke("Add", new object[] { User.Path.ToString() });
            }
        }
        public void RemoveGroup(string Name)
        {
            Group = AD.Children.Find(Name, "group");
            AD.Children.Remove(Group);
        }
        public void CloseConn()
        {
            //Cerrando conexiones
            if (AD != null)
            {
                //Cierra conexión a DirectoryService
                AD.Close();
            }
            if (User != null)
            {
                //Cerrar conexion hacia el usuario
                User.Close();
            }
            if (Group != null)
            {
                //Cerrar conexion hacia el grupo
                Group.Close();
            }
        }
        #endregion
    }
}