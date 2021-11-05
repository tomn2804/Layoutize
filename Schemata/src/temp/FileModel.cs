using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Schemata
{
	public class FileModel : Model
	{
		public FileModel(Model Outline)
			: base(outline)
		{
		}

		public override bool Exists => File.Exists(FullName);

		public static new Outline Build(ImmutableDefinition Outline)
		{
			Outline<Model> result = new();
			result["Name"] = Outline["Name"];
			result["Path"] = Outline["Path"];
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

		public static new Outline Build(ImmutableDefinition Outline)
		{
			Outline<FileModel> result = new();
			foreach (KeyValuePair<object, object> entry in Outline)
			{
				result[entry.Key] = entry.Value;
			}
			result["OnCreated"] = (Action<Model>)((model) =>
			{
				Console.WriteLine("Test");
				Outline.TryGetValue("OnCreated", out object? f);
				if (f is Action<Model> z)
				{
					z(model);
				}
			});
			return result;
		}
	}
}
