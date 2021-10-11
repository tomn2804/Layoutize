namespace SchemataPreview
{
    public abstract class FileSystemModel : Model
    {
        public abstract bool Exists { get; }

        public abstract void Create();

        public abstract void Delete();

        protected FileSystemModel(Schema schema)
            : base(schema)
        {
        }
    }
}
