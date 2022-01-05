# Templata
An object-oriented and schematic ways of building and managing Windows files/directories using PowerShell.

&nbsp;
# Quick preview:

## PowerShell input:
```PowerShell
using module Templata
using namespace Templata

$path = 'D:\'

$template = [DirectoryTemplate]@{
    Name = 'Bar'
    Children = @(
        [DirectoryTemplate]@{ Name = 'Hello' },
        [TextFileTemplate]@{ Name = 'World.txt' }
    )
    OnMounted = { 'Foo' | Write-Host }
}

$model = [Model+Workbench]::new($template).BuildTo($path)
$model.FullName | Write-Host
```

## Console output:
```PowerShell
Foo
D:\Bar
```

## Disk output:
![Quick preview output](./media/quick-preview-output.png)

&nbsp;
# More preview:

<details>
<summary>Extending from existing template</summary>

&nbsp;
# PowerShell input:
```PowerShell
using module Templata
using namespace Templata
using namespace System.Collections

class MyTextFileTemplate : Template[FileModel] {
    MyTextFileTemplate([IDictionary]$details) : base($details) {}

    [Blueprint]ToBlueprint() {
        $inputText = $this.Details['Text']
        $wrappedText = "Hello, $inputText"
        return [TextFileTemplate]@{ Name = $this.Details['Name']; Text = $wrappedText }
    }
}

$path = 'D:\'

$template = [MyTextFileTemplate]@{
    Name = 'MyTextFileTemplate.txt'
    Text = 'World!'
}

$model = [Model+Workbench]::new($template).BuildTo($path)
```

## Disk output:
![Quick preview output](./media/advanced-preview-output.png)

</details>
