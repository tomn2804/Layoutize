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
        if (((string)Details[RequiredDetails.Name]).IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        {
            throw new ArgumentException("Property 'Name' cannot contain invalid characters.", nameof(details));
        }
    }

    protected override Blueprint ToBlueprint()
    {
        return new BlankTemplate(Details);
    }
}
