using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;

namespace SchemataPreview
{
	public class FileModel : Model
	{
		public FileModel(Model schema)
			: base(schema)
		{
		}

		public override bool Exists => File.Exists(FullName);

		public static new Schema Build(ImmutableSchema schema)
		{
			Schema<Model> result = new();
			result["Name"] = schema["Name"];
			result["Path"] = schema["Path"];
			result["OnCreated"] = (Action<Model>)((model) =>
			{
				Console.WriteLine("Create file");
				//File.Create(model).Dispose();
			});
			//result["OnDeleted"] = (Action<Model>)((model) =>
			//{
			//	FileSystem.DeleteFile(model, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
			//});
			return result;
		}
	}

	public class TextModel : Model<FileModel>
	{
		public TextModel(FileModel model)
			: base(model)
		{
		}

		public override bool Exists => File.Exists(FullName);

		public static new Schema Build(ImmutableSchema schema)
		{
			Schema<FileModel> result = new();
			foreach (KeyValuePair<object, object> entry in schema)
			{
				result[entry.Key] = entry.Value;
			}
			result["OnCreated"] = (Action<Model>)((model) =>
			{
				Console.WriteLine("Test");
				schema.TryGetValue("OnCreated", out object? f);
				if (f is Action<Model> z)
				{
					z(model);
				}
			});
			return result;
		}
	}
}
