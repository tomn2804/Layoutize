using System.Collections;

namespace Schemata;

public sealed class StrictTextFileTemplate : Template<FileModel>
{
    public StrictTextFileTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return new TextFileTemplate(Details);
    }
}
