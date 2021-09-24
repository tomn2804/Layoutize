#Requires -Module SchemataPreview
using namespace SchemataPreview

Set-Location -Path $PSScriptRoot

Get-ChildItem -Path '.\tests\temp' | Remove-Item -Recurse
Get-Item -Path '.\tests' | Get-ChildItem -Include '*.ps1' | ForEach-Object -Process {
	"Invoking [$($_.Name)]" | Write-Host
	&"$_" | Out-Null
	Write-Host
}
