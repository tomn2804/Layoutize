using System;

namespace SchemataPreview
{
    public class TraversalProperty : DefaultProperty<PipelineTraversalOption>
    {
        public TraversalProperty(PipelineTraversalOption value)
            : base(value)
        {
        }

        public TraversalProperty(Schema props)
            : base(props)
        {
        }

        public override string Key => "Traversal";

        protected override PipelineTraversalOption GetDefaultValue()
        {
            return PipelineTraversalOption.Default;
        }
    }
}
