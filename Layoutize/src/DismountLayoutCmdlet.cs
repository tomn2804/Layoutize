using Layoutize.Elements;
using System.Management.Automation;

namespace Layoutize;

[Cmdlet(VerbsData.Dismount, "Layout")]
public class DismountLayoutCmdlet : Cmdlet
{
    [Parameter(Mandatory = true)]
    [ValidateNotNull]
    public IBuildContext Context { get; set; } = null!;

    protected override void ProcessRecord()
    {
        base.ProcessRecord();
        Context.Element.Unmount();
        WriteObject(Context);
    }
}
