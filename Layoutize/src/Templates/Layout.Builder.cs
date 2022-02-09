using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Templatize.Templates;

public sealed partial class Layout
{
    public class Builder
    {
        public Builder()
        {
            Attributes = ImmutableDictionary.CreateBuilder<object, object>();
            Templates = ImmutableList.CreateBuilder<Template>();
        }

        public Builder(Layout layout)
        {
            Attributes = layout.Attributes.ToDictionary(entry => entry.Key, entry => entry.Value);
            Templates = layout.Templates.ToList();
        }

        public IDictionary<object, object> Attributes { get; set; }

        public Layout ToLayout()
        {
            return new() { Attributes = Attributes.ToImmutableDictionary(), Templates = Templates.ToImmutableList() };
        }

        internal IList<Template> Templates { get; set; }
    }
}
