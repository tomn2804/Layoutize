[CmdletBinding()]
param (
	[Parameter(Mandatory)]
	[ValidateNotNullOrEmpty()]
	[string]
	$TargetPath,

	[string]
	[ValidateNotNullOrEmpty()]
	$PSModulePath = ($Env:PSModulePath -split ';' | Select-Object -First 1)
)

Copy-Item -Path $TargetPath -Destination (
	New-Item -Path $PSModulePath -Name (Split-Path -Path $TargetPath -LeafBase) -ItemType 'Directory' -Force
)
