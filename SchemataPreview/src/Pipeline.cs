using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchemataPreview
{
	public class Pipeline
	{
		public Pipeline(Model model)
		{
			Model = model;
		}

		public void Invoke(object key)
		{
			PipeSegment segment = new();
			segment.Extend(Model.PipeAssembly[key]);
			switch (Model.Traversal)
			{
				case ("ReversePostOrder"):
					TraverseReversePostOrder(key, Model.Children);
					break;

				case ("ReversePreOrder"):
				default:
					TraverseReversePreOrder(key, Model.Children);
					break;
			}
			segment.Flush();
		}

		public async Task InvokeAsync(object key)
		{
			await Task.Run(() => Invoke(key));
		}

		public async Task InvokeParallel(object key)
		{
			await Task.Run(() =>
			{
				PipeSegment segment = new();
				segment.Extend(Model.PipeAssembly[key]);
				TraverseReversePreOrderParallel(key, Model.Children);
				segment.Flush();
			});
		}

		protected Model Model { get; init; }

		private static void TraverseReversePostOrder(object key, ModelSet children)
		{
			Stack<PipeSegment> segments = new();
			foreach (Model child in children)
			{
				PipeSegment segment = new();
				segments.Push(segment);
				segment.Extend(child.PipeAssembly[key]);
			}
			foreach (Model child in children)
			{
				TraverseReversePostOrder(key, child.Children);
				segments.Pop().Flush();
			}
		}

		private static void TraverseReversePreOrder(object key, ModelSet children)
		{
			foreach (Model child in children)
			{
				PipeSegment segment = new();
				segment.Extend(child.PipeAssembly[key]);
				TraverseReversePreOrder(key, child.Children);
				segment.Flush();
			}
		}

		private static void TraverseReversePreOrderParallel(object key, ModelSet children)
		{
			List<Task> tasks = new();
			foreach (Model child in children)
			{
				tasks.Add(Task.Run(() =>
				{
					PipeSegment segment = new();
					segment.Extend(child.PipeAssembly[key]);
					TraverseReversePreOrderParallel(key, child.Children);
					segment.Flush();
				}));
			}
			Task.WaitAll(tasks.ToArray());
		}
	}
}
