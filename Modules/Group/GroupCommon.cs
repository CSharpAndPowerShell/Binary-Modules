using System.DirectoryServices;

namespace User
{
    class GroupCommon
    {
        #region Objects
        private DirectoryEntry AD = new DirectoryEntry("WinNT://" + System.Environment.MachineName + ",computer");
        private DirectoryEntry Group;
        private Common.Common CC = new Common.Common();
        #endregion
        #region Methods
        public void NewGroup(string Name, string Description)
        {
            CC.NewGroup(Name, Description);
        }
        public void RemoveGroup(string Name)
        {
            Group = AD.Children.Find(Name);
            AD.Children.Remove(Group);
        }
        public void AddToGroup(string Name, string Group)
        {
            CC.AddToGroup(Name, Group);
        }
        public void CloseConn()
        {
            CC.CloseConn(AD);
            CC.CloseConn(Group);
        }
        #endregion
    }
}
