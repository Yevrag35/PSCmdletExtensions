$curDir = Split-Path -Parent $MyInvocation.MyCommand.Definition;
$myDesktop = [System.Environment]::GetFolderPath("Desktop")

Import-Module "$curDir\MG.Posh.Extensions.dll" -ErrorAction Stop
Import-Module "$curDir\TestingModule-NETFramework.dll" -ErrorAction Stop

Push-Location $([System.Environment]::GetFolderPath("Desktop"))