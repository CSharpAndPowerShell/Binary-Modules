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

using System.Management;

namespace RestorePoint
{
    public class RestorePointCommon
    {
        //private ManagementObject SystemRestore =
        //    new ManagementObject("root\\DEFAULT",
        //        "SystemRestore",
        //        null);
        private static ManagementScope MScope =
            new ManagementScope(@"\\localhost\root\default");
        private static ManagementPath MPath =
            new ManagementPath("SystemRestore");
        private static ObjectGetOptions Options =
            new ObjectGetOptions();
        ManagementClass SystemRestore =
            new ManagementClass(MScope,MPath,Options);
        private static ManagementBaseObject Parameters;

        public enum RestorePointType
        {
            APPLICATION_INSTALL = 0,
            APPLICATION_UNINSTALL = 1,
            CANCELLED_OPERATION = 13,
            DEVICE_DRIVER_INSTALL = 10,
            MODIFY_SETTINGS = 12
        }

        public enum EventType
        {
            BEGIN_SYSTEM_CHANGE = 100,
            END_SYSTEM_CHANGE = 101,
            BEGIN_NESTED_SYSTEM_CHANGE = 102,
            END_NESTED_SYSTEM_CHANGE = 103
        }

        public void NewRestorePoint(string description, RestorePointType _RestorePointType, EventType _EventType)
        {
            // Crea un punto de restauración
            // Obteniendo parámetros del método "CreateRestorePoint"
            Parameters = SystemRestore.GetMethodParameters("CreateRestorePoint");

            // Parametros para el método "CreateRestorePoint"
            Parameters["Description"] = description;
            Parameters["RestorePointType"] = _RestorePointType;
            Parameters["EventType"] = _EventType;

            // Creando punto de restauración
            SystemRestore.InvokeMethod("CreateRestorePoint", Parameters, null);
        }

        public void SetRestorePoint(string on_off, string drive)
        {
            // Obteniendo parámetros del método "Enable" o "Disable"
            // Que serían los únicos valores permitidos por parametro
            Parameters = SystemRestore.GetMethodParameters(on_off);

            // Parametros para el método
            Parameters["Drive"] = drive;

            // Activando o desactivando la restauración en la unidad especificada
            SystemRestore.InvokeMethod(on_off, Parameters, null);
        }
    }
}
