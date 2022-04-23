using System.Collections.Immutable;

namespace Layoutize;

public interface IBuildContext
{
    ImmutableDictionary<object, object> Scope { get; }
}
