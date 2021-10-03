using System.Collections;

namespace SchemataPreview
{
    public partial class Definition : ImmutableDefinition.Builder
    {
        public Definition()
        {
        }

        public Definition(IDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
