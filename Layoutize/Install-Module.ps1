param (
	[Parameter(Mandatory)]
	[ValidateNotNullOrEmpty()]
	[string]$TargetPath
)

$PSModulePath = $env:PSModulePath -split ';' | Select-Object -First 1
$ProjectName = Split-Path -Path $TargetPath -LeafBase
$ProjectModulePath = New-Item -Path $PSModulePath -Name $ProjectName -ItemType 'Directory' -Force

Copy-Item -Path $TargetPath -Destination $ProjectModulePath
