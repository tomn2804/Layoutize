#Requires -Module SchemataPreview
using namespace SchemataPreview

$name = 'TextModel'
$contents = 'single line test'

$schema = [Schema[TextModel]]@{
	Name = "$name.txt"
	Path = 'D:\'
	Contents = 'single line test'
}
$schema.Build()

if (Test-Path -Path "D:\$($name).txt") {
	Write-Host "[$name]: Mount test Passed"
} else {
	Write-Host "[$name]: Mount test Failed"
}

if (
	(Get-Content -Path "D:\$($name).txt") -eq $contents
) {
	Write-Host "[$name]: Contents property test Passed"
} else {
	Write-Host "[$name]: Contents property test Failed"
}
