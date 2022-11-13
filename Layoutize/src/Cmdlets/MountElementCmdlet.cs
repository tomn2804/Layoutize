using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using Layoutize.Annotations;
using Layoutize.Elements;
using Layoutize.Layouts;
using Layoutize.Utils;

namespace Layoutize.Cmdlets;

[Cmdlet(VerbsData.Mount, nameof(Element))]
public class MountElementCmdlet : PSCmdlet
{
	[Parameter(Mandatory = true, Position = 0)]
	[ValidateNotNullOrEmpty]
	public string Path { get; init; } = null!;

	[Parameter(Mandatory = true, Position = 1)]
	[ValidateNotNull]
	public Layout Layout { get; init; } = null!;

	protected override void ProcessRecord()
	{
		base.ProcessRecord();
		Model.Validate(Layout);
		var rootElement = new RootDirectoryLayout { FullName = GetFullyQualifiedPath(), Children = new[] { Layout } }.CreateElement();
		rootElement.Mount();
		WriteObject(rootElement);
	}

	private string GetFullyQualifiedPath()
	{
		var path = System.IO.Path.IsPathFullyQualified(Path)
			? Path
			: System.IO.Path.Combine(SessionState.Path.CurrentLocation.Path, Path);
		PathAttribute.Validate(path);
		path = System.IO.Path.GetFullPath(path);
		Debug.Assert(PathAttribute.IsValid(path));
		if (!Directory.Exists(path)) throw new PSArgumentException($"{nameof(Path)} does not exists.", nameof(Path));
		return path;
	}
}
