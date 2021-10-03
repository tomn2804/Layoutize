namespace SchemataPreview
{
    public class TraversalProperty : DefaultProperty<PipelineTraversalOption>
    {
        public TraversalProperty(ImmutableDefinition definition)
            : base(definition)
        {
        }

        public override string Key => "Traversal";
        protected override PipelineTraversalOption DefaultValue => PipelineTraversalOption.Default;
    }
}
