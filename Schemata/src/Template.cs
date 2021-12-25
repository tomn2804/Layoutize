using System;
using System.Collections;

namespace Schemata;

public class BlankTemplate : Template<Model>
{
    public BlankTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return base.ToBlueprint();
    }
}

public class FileTemplate : Template<FileModel>
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

public abstract class Template<T> : Blueprint.Template where T : Model
{
    public override Type ModelType => typeof(T);

    protected Template(IEnumerable details)
            : base(details)
    {
    }
}

public class TextFileTemplate : Template<FileModel>
{
    public TextFileTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return new FileTemplate(Details);
    }
}

public class StrictTextFileTemplate : Template<FileModel>
{
    public StrictTextFileTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return new TextFileTemplate(Details);
    }
}
