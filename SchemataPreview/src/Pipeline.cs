using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SchemataPreview
{
	public class Pipeline
	{
		public Pipeline(Model model)
		{
			Model = model;
		}

		public Model Model { get; }

		public void Invoke(object key)
		{
			Pipe pipe = new(Model);
			if (pipe.Extend(key))
			{
				Debug.Assert(Model.Children != null);
				switch (Model.Traversal)
				{
					case PipelineTraversalOption.PostOrder:
						PipelineSequential.TraverseReversePostOrder(key, Model.Children);
						break;

					case PipelineTraversalOption.PreOrder:
					default:
						PipelineSequential.TraverseReversePreOrder(key, Model.Children);
						break;
				}
			}
			pipe.Flush();
		}

		public async Task InvokeAsync(object key)
		{
			await Task.Run(() => Invoke(key));
		}

		public async Task InvokeParallel(object key)
		{
			await Task.Run(() =>
			{
				Pipe pipe = new(Model);
				if (pipe.Extend(key))
				{
					Debug.Assert(Model.Children != null);
					PipelineParallel.TraverseReversePreOrderParallel(key, Model.Children);
				}
				pipe.Flush();
			});
		}
	}
}
