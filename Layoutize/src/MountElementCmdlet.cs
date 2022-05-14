using Layoutize.Attributes;
using Layoutize.Elements;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;

namespace Layoutize;

[Cmdlet(VerbsData.Mount, nameof(Element))]
public class MountElementCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 1)]
    [ValidateNotNull]
    public Layout Layout { get; set; } = null!;

    [Parameter(Mandatory = true, Position = 0)]
    [ValidateNotNullOrEmpty]
    public string Path { get; set; } = null!;

    private string FullPath
    {
        get
        {
            string fullPath = Path;
            if (!System.IO.Path.IsPathFullyQualified(fullPath))
            {
                fullPath = System.IO.Path.Combine(SessionState.Path.CurrentLocation.Path, fullPath);
            }
            Attributes.Path.Validate(fullPath);
            fullPath = System.IO.Path.GetFullPath(fullPath);
            Debug.Assert(Attributes.Path.IsValid(fullPath));
            return fullPath;
        }
    }

    protected override void ProcessRecord()
    {
        base.ProcessRecord();
        Debug.Assert(Layout is not RootDirectoryLayout);
        Debug.Assert(Attributes.Path.IsValid(FullPath));
        DirectoryInfo rootDirectory = Directory.CreateDirectory(FullPath);
        ImmutableDictionary<object, object> attributes = ImmutableDictionary.CreateRange(new[]
        {
            KeyValuePair.Create<object, object>(nameof(Name), rootDirectory.Name),
            KeyValuePair.Create<object, object>(nameof(Attributes.Path), rootDirectory.Parent?.FullName ?? string.Empty),
            KeyValuePair.Create<object, object>(nameof(Children), Layout),
        });
        DirectoryElement rootElement = new RootDirectoryLayout(attributes).CreateElement();
        rootElement.Mount(null);
        Debug.Assert(!rootElement.IsDisposed);
        Debug.Assert(rootElement.IsMounted);
        WriteObject(rootElement);
    }
}
