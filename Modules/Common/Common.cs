using System.Collections;
using System.DirectoryServices;
using System.Management;

namespace Common
{
    public class Common
    {
        #region Objects
        private DirectoryEntry AD = new DirectoryEntry("WinNT://" + System.Environment.MachineName + ",computer");
        private DirectoryEntry User;
        private DirectoryEntry Group;
        #endregion
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
        public void CloseConn(DirectoryEntry DE)
        {
            //Cerrando conexiones
            if (DE != null)
            {
                //Cierra conexión a DirectoryService
                DE.Close();
            }
        }
    }
}