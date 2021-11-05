using System;

namespace Schemata
{
    public class TraversalProperty : DefaultProperty<PipelineTraversalOption>
    {
        public TraversalProperty(PipelineTraversalOption value)
            : base(value)
        {
        }

        public TraversalProperty(Schema outline)
            : base(outline)
        {
        }

        public override string Key => "Traversal";

        protected override PipelineTraversalOption GetDefaultValue()
        {
            return PipelineTraversalOption.Default;
        }
    }
}
