#Requires -Module SchemataPreview
using namespace SchemataPreview

$name = 'DirectoryModel'
$schema = [Schema[DirectoryModel]]@{
	Name = $name
	Path = (Split-Path -Path "D:\$name" -Parent)
	Children = [Schema[DirectoryModel]]@{
		Name = 1
		Children = @(
			[Schema[DirectoryModel]]@{
				Name = 2
				Children = [Schema[DirectoryModel]]@{ Name = 3 }
			},
			[Schema[DirectoryModel]]@{ Name = 4 },
			[Schema[DirectoryModel]]@{ Name = 5 }
		)
	}
}
$schema.Build()

if (
	(Test-Path -Path @(
		"D:\$name\1",
		"D:\$name\1\2",
		"D:\$name\1\2\3",
		"D:\$name\1\4",
		"D:\$name\1\5"
	)) -notcontains $false
) {
	Write-Host "[$name]: Mount test Passed"
} else {
	Write-Host "[$name]: Mount test Failed"
}
