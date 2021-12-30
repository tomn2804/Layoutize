using System;
using System.Collections;
using System.Linq;
using System.IO;

namespace Schemata;

public sealed class FileTemplate : Template<FileModel>
{
    public FileTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        Blueprint blueprint = new BlankTemplate(Details);
        if (((string)blueprint.Details[RequiredDetails.Name]).IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        {
            throw new ArgumentException("Details property 'Name' cannot contain invalid characters.", "details");
        }
        return blueprint;
    }
}
