using Layoutize.Elements;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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
        ImmutableDictionary<object, object> attributes = ImmutableDictionary.CreateRange(new[]
        {
            KeyValuePair.Create<object, object>("FullName", Path),
            KeyValuePair.Create<object, object>("Children", Layout)
        });
        DirectoryLayout rootLayout = new(attributes);
        DirectoryElement element = rootLayout.CreateElement();
        element.MountTo(null);
        Debug.Assert(!element.IsDisposed);
        Debug.Assert(element.IsMounted);
        WriteObject(element);
    }
}
