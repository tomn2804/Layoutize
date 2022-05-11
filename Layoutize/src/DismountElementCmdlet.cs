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
        Element rootElement = Context.Element;
        Debug.Assert(rootElement.Layout is RootDirectoryLayout);
        if (rootElement.IsDisposed)
        {
            throw new PSObjectDisposedException(nameof(Context));
        }
        if (!rootElement.IsMounted)
        {
            rootElement.Unmount();
        }
        Debug.Assert(!rootElement.IsMounted);
        WriteObject(Context);
    }
}
