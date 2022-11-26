using System.Diagnostics;
using System.Management.Automation;
using Layoutize.Elements;

namespace Layoutize.Cmdlets;

[Cmdlet(VerbsData.Dismount, nameof(LocalElement))]
public class DismountElementCmdlet : Cmdlet
{
	[Parameter(Mandatory = true, Position = 0)]
	[ValidateNotNull]
	public IBuildContext Context { get; init; } = null!;

	protected override void ProcessRecord()
	{
		base.ProcessRecord();
		var element = Context.Element;
		Debug.Assert(element is RootDirectoryElement);
		if (element.IsMounted) element.Unmount();
		WriteObject(element);
	}
}
