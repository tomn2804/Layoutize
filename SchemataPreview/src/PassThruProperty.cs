namespace SchemataPreview
{
    public class PassThruProperty : DefaultProperty<bool>
    {
        public PassThruProperty(ImmutableDefinition definition)
            : base(definition)
        {
        }

        public override string Key => "PassThru";
        protected override bool DefaultValue => false;
    }
}
