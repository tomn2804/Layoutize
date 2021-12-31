using System.Collections;

namespace Schemata;

public sealed class TextFileTemplate : Template<FileModel>
{
    public TextFileTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return new FileTemplate((IDictionary)Details);
    }
}
