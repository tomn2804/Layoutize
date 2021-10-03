using System;
using System.Collections;

namespace SchemataPreview
{
    public class FileSchema : Schema<FileModel>
    {
        public FileSchema(Definition definition)
            : base(definition)
        {
        }

        protected override Schema Build()
        {
            Definition definition = new();
            definition["OnCreated"] = (Action<FileModel>)((model) =>
            {
                model.Create();
                ((Action<Model>?)definition["OnCreated"])?.Invoke(model);
            });
            definition["OnCreated"] = (Action<FileModel>)((model) =>
            {
                model.Delete();
                ((Action<Model>?)definition["OnCreated"])?.Invoke(model);
            });
            definition.Add(PropsOperator.Spread, definition);
            return new FileSchema(definition);
        }
    }
}
