using System;

namespace Layoutize.Elements;

public interface IBuildContext : IDisposable
{
    internal Element Element { get; }
}
