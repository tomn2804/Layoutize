#Requires -Module SchemataPreview
using namespace SchemataPreview

$name = 'DirectoryModel'

$tests = @(
	@{
		Name = 'Basic'
		Schema = [Schema[DirectoryModel]]@{
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
		ExpectError = $false
		ExpectedPaths = @(
			"D:\$name\1",
			"D:\$name\1\2",
			"D:\$name\1\2\3",
			"D:\$name\1\4",
			"D:\$name\1\5"
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
