using IWshRuntimeLibrary;
using System;

namespace NetworkDrive
{
    public class NetworkDriveCommon
    {
        #region Objects
        IWshNetwork_Class network = new IWshNetwork_Class();
        Drive.DriveCommon RD = new Drive.DriveCommon();
        #endregion
        public void NewNetworkDrive(char letter, string path, string user, string password, string name)
        {
            if (user != null)
            {
                network.MapNetworkDrive(letter + ":", path, Type.Missing, user, password);
            }
            else
            {
                network.MapNetworkDrive(letter + ":", path);
            }
            if (name != null)
            {
                RD.RenameDrive(letter, name);
            }
        }
        public void RemoveNetworkDrive(char letter)
        {
            network.RemoveNetworkDrive(letter + ":", true);
        }
    }
}
