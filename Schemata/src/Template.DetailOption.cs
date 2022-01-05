namespace Schemata;

public abstract partial class Template
{
    public class DetailOption
    {
        public const string Name = $"__{nameof(Model.Name)}";
        public const string Path = $"__{nameof(Model.Path)}";
        public const string Priority = $"__{nameof(Model.Priority)}";

        public const string OnCreating = $"__{nameof(OnCreating)}";
        public const string OnCreated = $"__{nameof(OnCreated)}";

        public const string OnMounting = $"__{nameof(OnMounting)}";
        public const string OnMounted = $"__{nameof(OnMounted)}";
    }
}
