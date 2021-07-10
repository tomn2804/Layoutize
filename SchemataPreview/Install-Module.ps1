[CmdletBinding()]
param (
	[Parameter(Mandatory)]
	[string]
	$TargetPath
)

Copy-Item -Path $TargetPath -Destination "C:\Users\Tom\Documents\PowerShell\Modules\$(Split-Path -Path $TargetPath -LeafBase)" -Force

test({

	})