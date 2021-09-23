using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SchemataPreview
{
	public class Pipeline : PipelineBase
	{
		public Pipeline(Model model)
			: base(model)
		{
		}

		public static void TraverseReversePostOrder(object key, IEnumerable<Model> children)
		{
			Queue<Tuple<Model, Pipe>> pipes = new();
			foreach (Model child in children)
			{
				Pipe pipe = new();
				if (child.PipeAssembly.Build(key, pipe))
				{
					pipes.Enqueue(new(child, pipe));
					break;
				}
				pipe.Flush();
			}
			while (pipes.Count != 0)
			{
				Tuple<Model, Pipe> pipe = pipes.Dequeue();
				Debug.Assert(pipe.Item1.Children != null);
				TraverseReversePostOrder(key, pipe.Item1.Children);
				pipe.Item2.Flush();
			}
		}

		public static void TraverseReversePreOrder(object key, IEnumerable<Model> children)
		{
			foreach (Model child in children)
			{
				Pipe pipe = new();
				if (child.PipeAssembly.Build(key, pipe))
				{
					Debug.Assert(child.Children != null);
					TraverseReversePreOrder(key, child.Children);
				}
				pipe.Flush();
			}
		}

		public void Invoke(object key)
		{
			Pipe pipe = new();
			if (Model.PipeAssembly.Build(key, pipe))
			{
				Debug.Assert(Model.Children != null);
				switch (Model.Traversal)
				{
					case PipelineTraversalOption.PostOrder:
						TraverseReversePostOrder(key, Model.Children);
						break;

					case PipelineTraversalOption.PreOrder:
					default:
						TraverseReversePreOrder(key, Model.Children);
						break;
				}
			}
			pipe.Flush();
		}
	}
}
