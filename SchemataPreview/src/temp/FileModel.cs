using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;

namespace SchemataPreview
{
	public class FileModel : Model
	{
		public FileModel(Model Props)
			: base(props)
		{
		}

		public override bool Exists => File.Exists(FullName);

		public static new Props Build(ImmutableDefinition Props)
		{
			Props<Model> result = new();
			result["Name"] = Props["Name"];
			result["Path"] = Props["Path"];
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

		public static new Props Build(ImmutableDefinition Props)
		{
			Props<FileModel> result = new();
			foreach (KeyValuePair<object, object> entry in Props)
			{
				result[entry.Key] = entry.Value;
			}
			result["OnCreated"] = (Action<Model>)((model) =>
			{
				Console.WriteLine("Test");
				Props.TryGetValue("OnCreated", out object? f);
				if (f is Action<Model> z)
				{
					z(model);
				}
			});
			return result;
		}
	}
}
