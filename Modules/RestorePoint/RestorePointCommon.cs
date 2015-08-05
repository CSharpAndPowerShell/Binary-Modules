using System;
using System.Management;

namespace RestorePoint
{
    public class RestorePointCommon
    {
        public void create_restorepoint(string description)
        {
            //Inicializando el objeto
            ManagementObject classInstance = new ManagementObject("root\\DEFAULT", "SystemRestore", null);
            // Obteniendo parámetros
            ManagementBaseObject inParams = classInstance.GetMethodParameters("CreateRestorePoint");
            // Parametros para el método 'CreateRestorePoint'
            inParams["Description"] = description;
            inParams["RestorePointType"] = 1;
            // Creando punto de restauración
            ManagementBaseObject outParams = classInstance.InvokeMethod("CreateRestorePoint", inParams, null);
        }
    }
}
