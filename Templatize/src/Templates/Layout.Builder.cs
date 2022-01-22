using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Templatize.Templates;

public partial class Layout
{
    public class Builder
    {
        public Builder()
        {
            Properties = ImmutableDictionary.CreateBuilder<object, object>();
            Templates = ImmutableList.CreateBuilder<Template>();
        }

        public Builder(Layout layout)
        {
            Properties = layout.Properties.ToDictionary(entry => entry.Key, entry => entry.Value);
            Templates = layout.Templates.ToList();
        }

        public IDictionary<object, object> Properties { get; set; }

        public Layout ToContext()
        {
            return new() { Properties = Properties.ToImmutableDictionary(), Templates = Templates.ToImmutableList() };
        }

        internal IList<Template> Templates { get; set; }
    }
}
