#Requires -Module SchemataPreview
using namespace SchemataPreview

Set-Location -Path $PSScriptRoot

$name = 'StrictTextModel'
$tempPath = "$PSScriptRoot\temp"
$localPath = "$PSScriptRoot\temp\$name"
