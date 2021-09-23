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
					Pipe pipe = new(child);
					if (pipe.Extend(key))
					{
						Debug.Assert(child.Children != null);
						TraverseReversePreOrderParallel(key, child.Children);
					}
					pipe.Flush();
				}));
			}
			Task.WaitAll(tasks.ToArray());
		}
	}
}
