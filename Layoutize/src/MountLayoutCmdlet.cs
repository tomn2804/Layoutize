using Layoutize.Elements;
using System.IO;
using System.Management.Automation;

namespace Layoutize;

[Cmdlet(VerbsData.Mount, "Layout")]
public class MountLayoutCmdlet : Cmdlet
{
    [Parameter(Mandatory = true, Position = 1)]
    [ValidateNotNull]
    public Layout Layout { get; set; } = null!;

    [Parameter(Mandatory = true, Position = 0)]
    [ValidateNotNullOrEmpty]
    public string Path { get; set; } = null!;

    protected override void ProcessRecord()
    {
        base.ProcessRecord();
        Directory.SetCurrentDirectory(Path);
        Element element = Layout.CreateElement();
        element.MountTo(null);
        WriteObject(element);
    }
}
