using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Schemata
{
	public static class PipelineSequential
	{
		public static void TraverseReversePostOrder(object key, IEnumerable<Model> children)
		{
			Queue<Tuple<Model, Pipe>> registries = new();
			foreach (Model child in children)
			{
				Pipe pipe = new(child);
				if (pipe.Extend(key))
				{
					registries.Enqueue(new(child, pipe));
				}
				else
				{
					pipe.Dispose();
				}
			}
			while (registries.Count != 0)
			{
				Tuple<Model, Pipe> registry = registries.Dequeue();
				Debug.Assert(registry.Item1.Children is not null);
				TraverseReversePostOrder(key, registry.Item1.Children);
				registry.Item2.Dispose();
			}
		}

		public static void TraverseReversePreOrder(object key, IEnumerable<Model> children)
		{
			foreach (Model child in children)
			{
				using Pipe pipe = new(child);
				if (pipe.Extend(key))
				{
					Debug.Assert(child.Children is not null);
					TraverseReversePreOrder(key, child.Children);
				}
			}
		}
	}
}
