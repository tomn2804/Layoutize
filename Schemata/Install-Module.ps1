[CmdletBinding()]
param (
	[Parameter(Mandatory)]
	[ValidateNotNullOrEmpty()]
	[string]
	$TargetPath
)

Copy-Item -Path $TargetPath -Destination ($Env:PSModulePath -split ';' | Select-Object -First 1 | New-Item -Name (Split-Path -Path $TargetPath -LeafBase) -ItemType 'Directory' -Force -PassThru)
