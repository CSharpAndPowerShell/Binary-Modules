using System;
using System.Management;

namespace RestorePoint
{
    public class RestorePointCommon
    {
        public void create_restorepoint()
        {
            try
            {
                ManagementObject classInstance =
                    new ManagementObject("root\\DEFAULT",
                    "SystemRestore", null);

                // Obtain in-parameters for the method
                ManagementBaseObject inParams =
                    classInstance.GetMethodParameters("CreateRestorePoint");

                // Add the input parameters.

                // Execute the method and obtain the return values.
                ManagementBaseObject outParams =
                    classInstance.InvokeMethod("CreateRestorePoint", inParams, null);

                // List outParams
                Console.WriteLine("Out parameters:");
                Console.WriteLine("ReturnValue: " + outParams["ReturnValue"]);
            }
            catch (ManagementException err)
            {
                MessageBox.Show("An error occurred while trying to execute the WMI method: " + err.Message);
            }
        }
    }
}
