using System.Collections;

namespace Schemata;

public sealed class FileTemplate : Template<FileModel>
{
    public FileTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return new BlankTemplate(Details);
    }
}
