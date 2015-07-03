using IWshRuntimeLibrary;
using System;

namespace NetworkDrive
{
    public class NetworkDriveCommon
    {
        #region Objects
        IWshNetwork_Class network = new IWshNetwork_Class();
        #endregion
        public void NewNetworkDrive(char letter, string path, string user, string password)
        {
            if (user != null)
            {
                network.MapNetworkDrive(letter + ":", path, Type.Missing, user, password);
            }
            else
            {
                network.MapNetworkDrive(letter + ":", path);
            }
        }
        public void RemoveNetworkDrive(char letter)
        {
            network.RemoveNetworkDrive(letter + ":", true);
        }
    }
}
