#Requires -Module SchemataPreview

class A : SchemataPreview.Models.Model {
	A($name) : base($name) {}
}

$a = [A]::new('a')
$b = [A]::new('b')
$c = [A]::new('c')
