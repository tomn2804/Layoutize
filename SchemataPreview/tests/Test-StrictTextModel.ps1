#Requires -Module SchemataPreview
using namespace SchemataPreview

$name = 'TextModel'

$tests = @(
	@{
		Name = 'Single Line'
		Schema = [Schema[TextModel]]@{
			Name = "$($name)1.txt"
			Path = 'D:\'
			Contents = 'single line test'
		}
		ExpectError = $false
		ExpectedPath = "D:\$($name)1.txt"
		ExpectedContents = 'single line test'
	},
	@{
		Name = 'Multiple Lines'
		Schema = [Schema[TextModel]]@{
			Name = "$($name)2.txt"
			Path = 'D:\'
			Contents = @(
				'Line 1',
				'Line 2',
				'Line 3'
			)
		}
		ExpectError = $false
		ExpectedPath = "D:\$($name)2.txt"
		ExpectedContents = @(
			'Line 1',
			'Line 2',
			'Line 3'
		)
	}
)

foreach ($test in $tests) {
	try {
		$model = $test.Schema.Build()
		if (!(Test-Path -Path $test.ExpectedPath)) {
			throw 'Test-Path -Path $test.ExpectedPath'
		}
		if (Compare-Object -ReferenceObject $test.ExpectedContents -DifferenceObject (Get-Content -Path $model)) {
			throw 'Compare-Object -ReferenceObject $test.ExpectedContents -DifferenceObject (Get-Content -Path $model)'
		}
		"[$($test.Name)]: Test passed" | Write-Host
	} catch {
		if (!$test.ExpectError) {
			"[$($test.Name)]: Unexpected error occured: '$_'" | Write-Host
		}
	}
}
