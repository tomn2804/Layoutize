using System;
using System.Collections.Generic;

namespace SchemataPreview
{
	public static class ListExtensions
	{
		public static void Add(this List<Model> value, params Model[] models)
		{
			foreach (Model model in models)
			{
				if (value.Find(m => m.Name == model.Name) != null)
				{
					throw new InvalidOperationException();
				}
				value.Add(model);
			}
			value.AddRange(models);
		}

		public static void Replace(this List<Model> value, params Model[] models)
		{
			foreach (Model model in models)
			{
				int index = value.FindIndex(m => m.Name == model.Name);
				if (index == -1)
				{
					throw new KeyNotFoundException();
				}
				value.RemoveAt(index);
				value.Insert(index, model);
			}
		}

		public static void Merge(this List<Model> value, params Model[] models)
		{
			foreach (Model model in models)
			{
				value.RemoveAt(value.FindIndex(m => m.Name == model.Name));
				value.Add(model);
			}
		}
	}
}
