using Layoutize.Elements;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;

namespace Layoutize;

[Cmdlet(VerbsData.Mount, "Element")]
public class MountElementCmdlet : Cmdlet
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
        DirectoryInfo directoryInfo = new(Path);
        ImmutableDictionary<object, object> attributes = ImmutableDictionary.CreateRange(new[]
        {
            KeyValuePair.Create<object, object>("Name", directoryInfo.Name),
            KeyValuePair.Create<object, object>("Path", directoryInfo.Parent?.FullName ?? string.Empty),
            KeyValuePair.Create<object, object>("Children", Layout)
        });
        DirectoryElement element = new RootDirectoryLayout(attributes).CreateElement();
        element.MountTo(null);
        Debug.Assert(!element.IsDisposed);
        Debug.Assert(element.IsMounted);
        WriteObject(element);
    }
}
