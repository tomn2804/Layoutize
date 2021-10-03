namespace SchemataPreview
{
    public abstract class FileSystemModel : Model
    {
        protected FileSystemModel(Schema schema)
            : base(schema)
        {
        }

        public abstract bool Exists { get; }

        public abstract void Create();

        public abstract void Delete();
    }
}
