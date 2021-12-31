using System.Collections;

namespace Schemata;

public sealed class DirectoryTemplate : Template<DirectoryModel>
{
    public DirectoryTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return new BlankTemplate((IDictionary)Details);
    }
}
