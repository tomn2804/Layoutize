namespace Schemata;

public sealed partial class TextFileTemplate
{
    public new class DetailOption : Template.DetailOption
    {
        public const string Text = $"__{nameof(Text)}";
    }
}
