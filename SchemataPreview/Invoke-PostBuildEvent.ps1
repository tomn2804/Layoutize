#Requires -Module SchemataPreview
using namespace SchemataPreview

Get-ChildItem -Path 'D:\' | Remove-Item -Recurse
Get-ChildItem -Path '.\tests' | ForEach-Object -Process { &"$_" | Out-Null }
