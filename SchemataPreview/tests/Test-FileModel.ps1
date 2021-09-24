#Requires -Module SchemataPreview
using namespace SchemataPreview

$name = 'FileModel'
$schema = [Schema[FileModel]]@{
	Name = "$name.txt"
	Path = 'D:\'
}
$schema.Build()

if (Test-Path -Path "D:\$($name).txt") {
	Write-Host "[$name]: Mount test Passed"
} else {
	Write-Host "[$name]: Mount test Failed"
}
