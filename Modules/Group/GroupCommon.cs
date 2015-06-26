using System.DirectoryServices;

namespace Group
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
                CloseConn(true, false, true);
            }
        }
        public void AddToGroup(string Name, string Grp)
        {
            try
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
            catch
            {
                CloseConn(true, true, true);
            }
        }
        public void RemoveGroup(string Name)
        {
            try
            {
                //Se elimina el grupo
                Group = AD.Children.Find(Name, "group");
                AD.Children.Remove(Group);
            }
            catch
            {
                CloseConn(true, false, true);
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
                Group.Close();
            }
        }
        #endregion
    }
}