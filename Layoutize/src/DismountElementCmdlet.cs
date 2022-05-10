using Layoutize.Elements;
using System.Diagnostics;
using System.Management.Automation;

namespace Layoutize;

[Cmdlet(VerbsData.Dismount, nameof(Element))]
public class DismountElementCmdlet : Cmdlet
{
    [Parameter(Mandatory = true)]
    [ValidateNotNull]
    public IBuildContext Context { get; set; } = null!;

    protected override void ProcessRecord()
    {
        base.ProcessRecord();
        Element element = Context.Element;
        element.Dispose();
        Debug.Assert(element.IsDisposed);
        Debug.Assert(!element.IsMounted);
        WriteObject(Context);
    }
}
