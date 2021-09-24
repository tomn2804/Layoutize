using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SchemataPreview
{
	public static class PipelineParallel
	{
		public static void TraverseReversePreOrderParallel(object key, IEnumerable<Model> children)
		{
			List<Task> tasks = new();
			foreach (Model child in children)
			{
				tasks.Add(Task.Run(() =>
				{
					using Pipe pipe = new(child);
					if (pipe.Extend(key))
					{
						Debug.Assert(child.Children != null);
						TraverseReversePreOrderParallel(key, child.Children);
					}
				}));
			}
			Task.WaitAll(tasks.ToArray());
		}
	}
}
