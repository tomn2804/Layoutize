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
        DirectoryInfo rootDirectory = Directory.CreateDirectory(Path);
        ImmutableDictionary<object, object> attributes = ImmutableDictionary.CreateRange(new[]
        {
            KeyValuePair.Create<object, object>("Name", rootDirectory.Name),
            KeyValuePair.Create<object, object>("Path", rootDirectory.Parent?.FullName ?? string.Empty),
            KeyValuePair.Create<object, object>("Children", Layout)
        });
        DirectoryElement rootElement = new RootDirectoryLayout(attributes).CreateElement();
        rootElement.Mount(null);
        Debug.Assert(!rootElement.IsDisposed);
        Debug.Assert(rootElement.IsMounted);
        WriteObject(rootElement);
    }
}
