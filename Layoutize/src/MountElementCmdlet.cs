using Layoutize.Elements;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;

namespace Layoutize;

[Cmdlet(VerbsData.Mount, nameof(Element))]
public class MountElementCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 1)]
    [ValidateNotNull]
    public Layout Layout { get; init; } = null!;

    [Parameter(Mandatory = true, Position = 0)]
    public string Path { get; init; } = string.Empty;

    private string FullName
    {
        get
        {
            string fullName = Path;
            if (!System.IO.Path.IsPathFullyQualified(fullName))
            {
                fullName = System.IO.Path.Combine(SessionState.Path.CurrentLocation.Path, fullName);
            }
            Contexts.Path.Validate(fullName);
            fullName = System.IO.Path.GetFullPath(fullName);
            Debug.Assert(Contexts.Path.IsValid(fullName));
            return fullName;
        }
    }

    protected override void ProcessRecord()
    {
        base.ProcessRecord();
        Debug.Assert(Contexts.Path.IsValid(FullName));
        DirectoryInfo rootDirectory = Directory.CreateDirectory(FullName);
        DirectoryElement rootElement = new RootDirectoryLayout()
        {
            Name = rootDirectory.Name,
            Path = rootDirectory.Parent?.FullName ?? string.Empty,
            Children = Layout,
        }.CreateElement();
        rootElement.Mount(null);
        Debug.Assert(rootElement.IsMounted);
        WriteObject(rootElement);
    }
}
