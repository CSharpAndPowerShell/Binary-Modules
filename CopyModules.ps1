<#
CSharpAndPowerShell Modules, tries to help Microsoft Windows admins to write automated scripts easier.
Copyright (C) 2015  Cristopher Robles Ríos

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
#>

#Copy Dll and xml
$RaizMod = $PSScriptRoot + "\Modules"
$ModulesFolder = Get-ChildItem -Path $RaizMod -Directory
Remove-Item -Path $PSScriptRoot\OUT -Force -Recurse -ErrorAction SilentlyContinue
Foreach ($MF in $ModulesFolder)
{
    mkdir -Path $PSScriptRoot\OUT\$MF -Force | Out-Null
    Copy-Item -Path $RaizMod\$MF\bin\Debug\$MF.dll -Destination $PSScriptRoot\Out\$MF -ErrorAction SilentlyContinue | Out-Null
    Copy-Item -Path $RaizMod\$MF\$MF.dll-Help.xml -Destination $PSScriptRoot\Out\$MF -ErrorAction SilentlyContinue | Out-Null
}