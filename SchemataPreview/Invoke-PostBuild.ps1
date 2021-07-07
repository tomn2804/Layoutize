﻿#Requires -Module SchemataPreview
using namespace SchemataPreview

class A : SchematicModel {
	A($name) : base($name) {}
}

$a = [A]::new('aa')
$a.Name | Write-Host
$b = [A]::new('bc')
$b.Name | Write-Host
$c = [A]::new('cc')
$c.Name | Write-Host
