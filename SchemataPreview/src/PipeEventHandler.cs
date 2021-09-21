using System;

namespace SchemataPreview
{
	public class PipeEventHandler
	{
		public static PipeEventHandler operator +(PipeEventHandler? lhs, Action rhs)
		{
			return lhs += (_) => rhs.Invoke();
		}

		public static PipeEventHandler operator +(PipeEventHandler? lhs, Action<PipeSegment> rhs)
		{
			if (lhs == null)
			{
				lhs = new();
			}
			lhs.Action += rhs;
			return lhs;
		}

		public void Invoke(PipeSegment segment)
		{
			Action?.Invoke(segment);
		}

		protected Action<PipeSegment>? Action { get; set; }
	}
}
