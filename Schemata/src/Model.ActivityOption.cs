namespace Schemata;

public abstract partial class Model
{
    public static class ActivityOption
    {
        public const string Create = $"__{nameof(Create)}";
        public const string Mount = $"__{nameof(Mount)}";
    }
}
