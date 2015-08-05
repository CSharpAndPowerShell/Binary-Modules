#Copiar Dll xml
$RaizMod = $PSScriptRoot + "\Modules"
$ModulesFolder = Get-ChildItem -Path $RaizMod -Directory
Remove-Item -Path $PSScriptRoot\OUT -Force -Recurse -ErrorAction SilentlyContinue
Foreach ($MF in $ModulesFolder)
{
    mkdir -Path $PSScriptRoot\OUT\$MF -Force | Out-Null
    Copy-Item -Path $RaizMod\$MF\bin\Debug\$MF.dll -Destination $PSScriptRoot\Out\$MF -ErrorAction SilentlyContinue | Out-Null
    Copy-Item -Path $RaizMod\$MF\$MF.dll-Help.xml -Destination $PSScriptRoot\Out\$MF -ErrorAction SilentlyContinue | Out-Null
}