using System.Diagnostics;
using System.Management.Automation;
using Layoutize.Contexts;
using Layoutize.Elements;

namespace Layoutize.Cmdlets;

[Cmdlet(VerbsData.Dismount, nameof(Element))]
public class DismountElementCmdlet : Cmdlet
{
	[Parameter(Mandatory = true, Position = 0)]
	[ValidateNotNull]
	public IBuildContext Context { get; init; } = null!;

	protected override void ProcessRecord()
	{
		base.ProcessRecord();
		var rootElement = Context.Element;
		Debug.Assert(rootElement is RootDirectoryElement);
		if (rootElement.IsMounted) rootElement.Unmount();
		Debug.Assert(!rootElement.IsMounted);
		WriteObject(rootElement);
	}
}
