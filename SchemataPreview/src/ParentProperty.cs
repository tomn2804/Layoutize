namespace SchemataPreview
{
    public class ParentProperty : NullableProperty<Model?>
    {
        public ParentProperty(ImmutableDefinition definition)
            : base(definition)
        {
        }

        public override string Key => "Parent";
    }
}
