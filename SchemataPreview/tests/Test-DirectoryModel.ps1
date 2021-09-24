#Requires -Module SchemataPreview
using namespace SchemataPreview

Set-Location -Path $PSScriptRoot

$name = 'DirectoryModel'
$tempPath = "$PSScriptRoot\temp"
$localPath = "$PSScriptRoot\temp\$name"

$tests = @(
	@{
		Name = 'Basic'
		Schema = [Schema[DirectoryModel]]@{
			Name = $name
			Path = $tempPath
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
		ExpectError = $false
		ExpectedPaths = @(
			"$localPath\1",
			"$localPath\1\2",
			"$localPath\1\2\3",
			"$localPath\1\4",
			"$localPath\1\5"
		)
	}
)

foreach ($test in $tests) {
	try {
		$model = $test.Schema.Build()
		if ((Test-Path -Path $test.ExpectedPaths) -contains $false) {
			throw 'Test-Path -Path $test.ExpectedPaths'
		}
		"[$($test.Name)]: Test passed" | Write-Host
	} catch {
		if (!$test.ExpectError) {
			"[$($test.Name)]: Unexpected error occured: $_" | Write-Host
		}
	}
}
