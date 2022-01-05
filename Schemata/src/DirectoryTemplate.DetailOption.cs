namespace Schemata;

public sealed partial class DirectoryTemplate
{
    public new class DetailOption : Template.DetailOption
    {
        public const string Children = $"__{nameof(DirectoryModel.Children)}";
        public const string Traversal = $"__{nameof(Traversal)}";
    }
}
