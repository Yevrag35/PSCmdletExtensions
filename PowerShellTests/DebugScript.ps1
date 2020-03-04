Import-Module "$PSScriptRoot\MG.Posh.Extensions-PowerShell.Testing.dll" -ErrorAction Stop
Set-Location -Path $([System.Environment]::GetFolderPath("Desktop"))

Function Write-Result()
{
	[CmdletBinding()]
	param
	(
		[Parameter(Mandatory=$true, Position = 0, ValueFromPipeline=$true)]
		[MG.Posh.Extensions.Tests.BoundTestResult] $Result
	)
	if ($Result.Passed)
	{
		Write-Host "PASS" -f Green
	}
	else
	{
		Write-Host "FAIL" -f Red
		Write-Warning $($Result | Out-String)
	}
}

#region TEST BOUND PARAMETERS

# TEST #1 - Returns all 'FALSE'
Write-Host "TEST BOUND PARAMETERS`n" -f Magenta

Write-Host "Testing None...`t`t" -f Cyan -NoNewLine
Test-BoundParameter | Write-Result

Write-Host "Testing Name...`t`t" -f Cyan -NoNewLine
Test-BoundParameter AnyNoAllNameNoInput -Name whatev | Write-Result

Write-Host "Testing InputObject...`t" -f Cyan -NoNewLine
Test-BoundParameter AnyNoAllNoNameInput -InputObject whatev | Write-Result

Write-Host "Testing All...`t`t" -f Cyan -NoNewLine
Test-BoundParameter AnyAllNameInput -Name whatev -InputObject cool | Write-Result

#endregion

#region TEST PIPED OBJECTS
Write-Host "`nTEST PIPED OBJECTS`n" -f Magenta



#endregion