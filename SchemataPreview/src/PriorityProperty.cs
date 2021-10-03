namespace SchemataPreview
{
    public class PriorityProperty : DefaultProperty<int>
    {
        public PriorityProperty(ImmutableDefinition definition)
            : base(definition)
        {
        }

        public override string Key => "Priority";
        protected override int DefaultValue => 0;
    }
}
