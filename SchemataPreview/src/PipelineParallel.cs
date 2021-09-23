using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SchemataPreview
{
	public class PipelineParallel : PipelineBase
	{
		public PipelineParallel(Model model)
			: base(model)
		{
		}

		public static void TraverseReversePreOrderParallel(object key, IEnumerable<Model> children)
		{
			List<Task> tasks = new();
			foreach (Model child in children)
			{
				tasks.Add(Task.Run(() =>
				{
					Pipe pipe = new();
					if (child.PipeAssembly.Build(key, pipe))
					{
						Debug.Assert(child.Children != null);
						TraverseReversePreOrderParallel(key, child.Children);
					}
					pipe.Flush();
				}));
			}
			Task.WaitAll(tasks.ToArray());
		}

		public async Task Invoke(object key)
		{
			await Task.Run(() =>
			{
				Pipe pipe = new();
				if (Model.PipeAssembly.Build(key, pipe))
				{
					Debug.Assert(Model.Children != null);
					TraverseReversePreOrderParallel(key, Model.Children);
				}
				pipe.Flush();
			});
		}
	}
}
