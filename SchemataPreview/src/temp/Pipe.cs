using System;
using System.Collections.Generic;

namespace SchemataPreview
{
	public partial class Pipe
	{
		public Pipe(Model model)
		{
			Model = model;
		}

		public Model Model { get; }

		public bool Extend(object key)
		{
			if (Model.PipeAssembly.TryGetValue(key, out PipeSegment? segment))
			{
				Extend(segment);
				return Model.Children is not null;
			}
			return Model.PassThru && (Model.Children is not null);
		}

		public void Extend(PipeSegment pipe)
		{
			if (pipe.OnProcessed is not null)
			{
				Callbacks.Push(pipe.OnProcessed);
			}
			pipe.OnProcessing.Invoke(this, EventArgs.Empty);
		}

		protected Stack<PipeEventHandler> Callbacks { get; } = new();
	}

	public partial class Pipe : IDisposable
	{
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool isDisposing)
		{
			if (isDisposing)
			{
				while (Callbacks.Count != 0)
				{
					Callbacks.Pop().Invoke(this, EventArgs.Empty);
				}
			}
		}

		~Pipe()
		{
			Dispose(false);
		}
	}
}
