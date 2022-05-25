using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using Layoutize.Elements;
using Layoutize.Layouts;

namespace Layoutize.Cmdlets;

[Cmdlet(VerbsData.Mount, nameof(Element))]
public class MountElementCmdlet : PSCmdlet
{
	[Parameter(Mandatory = true, Position = 1)]
	[ValidateNotNull]
	public Layout Layout { get; init; } = null!;

	[Parameter(Mandatory = true, Position = 0)]
	public string Path { get; init; } = string.Empty;

	protected override void ProcessRecord()
	{
		base.ProcessRecord();
		Debug.Assert(Contexts.Path.TryValidate(FullName));
		var rootDirectory = Directory.CreateDirectory(FullName);
		if (!rootDirectory.Exists) throw new PSArgumentException("Path does not exists.", nameof(Path));
		var rootElement = new RootDirectoryLayout
		{
			Name = rootDirectory.Name, Path = rootDirectory.Parent?.FullName ?? string.Empty, Children = Layout,
		}.CreateElement();
		rootElement.Mount(null);
		Debug.Assert(rootElement.IsMounted);
		WriteObject(rootElement);
	}

	private string FullName
	{
		get
		{
			var fullName = System.IO.Path.IsPathFullyQualified(Path)
				? Path
				: System.IO.Path.Combine(SessionState.Path.CurrentLocation.Path, Path);
			Contexts.Path.Validate(fullName);
			fullName = System.IO.Path.GetFullPath(fullName);
			Debug.Assert(Contexts.Path.TryValidate(fullName));
			return fullName;
		}
	}
}
