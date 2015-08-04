#Copiar Dll xml
$Raiz = Split-Path $MyInvocation.MyCommand.Definition
$RaizMod = $Raiz + "\Modules"
$ModulesFolder = Get-ChildItem -Path $RaizMod -Directory
Remove-Item -Path $Raiz\OUT -Force -Recurse -ErrorAction SilentlyContinue
Foreach ($MF in $ModulesFolder)
{
    mkdir -Path $Raiz\OUT\$MF -Force | Out-Null
    Copy-Item -Path $RaizMod\$MF\bin\Debug\$MF.dll -Destination $Raiz\Out\$MF | Out-Null
    Copy-Item -Path $RaizMod\$MF\$MF.dll-Help.xml -Destination $Raiz\Out\$MF | Out-Null
}