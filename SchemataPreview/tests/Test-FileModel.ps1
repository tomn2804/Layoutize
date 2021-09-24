#Requires -Module SchemataPreview
using namespace SchemataPreview

Set-Location -Path $PSScriptRoot

$name = 'FileModel'
$tempPath = "$PSScriptRoot\temp"
$localPath = "$PSScriptRoot\temp\$name"

$tests = @(
	@{
		Name = 'Basic'
		Schema = [Schema[FileModel]]@{
			Name = "$name.txt"
			Path = $tempPath
		}
		ExpectError = $false
		ExpectedPath = "$localPath.txt"
	}
)

foreach ($test in $tests) {
	try {
		$model = $test.Schema.Build()
		if (!(Test-Path -Path $test.ExpectedPath)) {
			throw 'Test-Path -Path $test.ExpectedPath'
		}
		"[$($test.Name)]: Test passed" | Write-Host
	} catch {
		if (!$test.ExpectError) {
			"[$($test.Name)]: Unexpected error occured: '$_'" | Write-Host
		}
	}
}
