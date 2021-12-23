using System;
using System.Collections;

namespace Schemata;

public abstract class Template<T> : Blueprint.Template where T : Model
{
    protected Template(IEnumerable details)
        : base(details)
    {
    }

    public override Type ModelType => typeof(T);
}

public class BlankTemplate : Template<Model>
{
    public BlankTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        Console.WriteLine(nameof(BlankTemplate));
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
        Console.WriteLine(nameof(FileTemplate));
        return new BlankTemplate(Details);
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
        Console.WriteLine(nameof(TextFileTemplate));
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
        Console.WriteLine(nameof(StrictTextFileTemplate));
        return new TextFileTemplate(Details);
    }
}
