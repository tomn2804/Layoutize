using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Layoutize.Layouts;

public sealed partial class Element
{
    public class Builder
    {
        public Builder()
        {
            Attributes = ImmutableDictionary.CreateBuilder<object, object>();
            Templates = ImmutableList.CreateBuilder<Layout>();
        }

        public Builder(Element layout)
        {
            Attributes = layout.Attributes.ToDictionary(entry => entry.Key, entry => entry.Value);
            Templates = layout.Templates.ToList();
        }

        public IDictionary<object, object> Attributes { get; set; }

        public Element ToLayout()
        {
            return new() { Attributes = Attributes.ToImmutableDictionary(), Templates = Templates.ToImmutableList() };
        }

        internal IList<Layout> Templates { get; set; }
    }
}
