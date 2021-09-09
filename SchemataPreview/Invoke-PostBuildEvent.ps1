#Requires -Module SchemataPreview
using namespace SchemataPreview

$Test = 'C:\Users\Tom\Desktop\Test'
$Test2 = 'C:\Users\Tom\Desktop\Test2'

Set-Location -Path $Test2

Get-ChildItem -Path $Test2 -Exclude 'Build-CurrentDirectorySchema.ps1', 'Get-CurrentDirectorySchema.ps1' | Remove-Item -Recurse

$m = &'.\Build-CurrentDirectorySchema.ps1'

Write-Host 'Initial test passed'
