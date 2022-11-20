using System.IO;
using System.Management.Automation;
using Layoutize.Annotations;
using Layoutize.Elements;
using Layoutize.Layouts;

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

	[FullName]
	private string FullName
	{
		get
		{
			var fullName = System.IO.Path.IsPathFullyQualified(Path)
				? Path
				: System.IO.Path.Combine(SessionState.Path.CurrentLocation.Path, Path);
			fullName = System.IO.Path.GetFullPath(fullName);
			if (!Directory.Exists(fullName)) throw new PSArgumentException($"{nameof(Path)} does not exists.", nameof(Path));
			this.ValidateMember(nameof(FullName), fullName);
			return fullName;
		}
	}

	protected override void ProcessRecord()
	{
		base.ProcessRecord();
		Layout.Validate();
		var element = new RootDirectoryLayout(FullName) { Children = new[] { Layout } }.CreateElement();
		element.Mount();
		WriteObject(element);
	}
}
