using Layoutize.Elements;
using System.Diagnostics;
using System.Management.Automation;

namespace Layoutize;

[Cmdlet(VerbsData.Dismount, nameof(Element))]
public class DismountElementCmdlet : Cmdlet
{
    [Parameter(Mandatory = true)]
    [ValidateNotNull]
    public IBuildContext Context { get; init; } = null!;

    protected override void ProcessRecord()
    {
        base.ProcessRecord();
        Element rootElement = Context.Element;
        if (!rootElement.IsMounted)
        {
            rootElement.Unmount();
        }
        Debug.Assert(!rootElement.IsMounted);
        WriteObject(Context);
    }
}
