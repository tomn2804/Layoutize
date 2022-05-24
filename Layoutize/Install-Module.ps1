param (
	[Parameter(Mandatory)]
	[ValidateNotNullOrEmpty()]
	[string]$TargetPath,

	[Parameter(Mandatory)]
	[ValidateNotNullOrEmpty()]
	[string]$ProjectName
)

$PSModulePath = $Env:PSModulePath -split ';' | Select-Object -First 1
$ProjectModulePath = New-Item -Path $PSModulePath -Name $ProjectName -ItemType 'Directory' -Force

Copy-Item -Path $TargetPath -Destination $ProjectModulePath
