using System;

namespace Schemata
{
    public class FileSchema : Schema<FileModel>
    {
        public FileSchema(ImmutableOutline outline)
            : base(outline)
        {
        }

        protected override Schema Build()
        {
            ImmutableOutline.Builder outline = new();
            outline["OnCreated"] = (Action<FileModel>)((model) =>
            {
                model.Create();
                ((Action<Model>?)Outline["OnCreated"])?.Invoke(model);
            });
            outline["OnDeleted"] = (Action<FileModel>)((model) =>
            {
                model.Delete();
                ((Action<Model>?)Outline["OnDeleted"])?.Invoke(model);
            });
            return new FileSchema(outline.MergeTo(Outline));
        }
    }
}
