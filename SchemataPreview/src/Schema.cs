using System;
using System.Collections;

namespace SchemataPreview
{
	public abstract class Schema
	{
		public abstract Model Mount();

		public ImmutableSchema ToImmutable()
		{
			return new(Props);
		}

		protected Schema(Hashtable props)
		{
			Props = props;
			State = Build();
		}

		protected Schema(Hashtable props, Schema state)
		{
			Props = props;
			State = state;
		}

		protected Hashtable Props { get; }

		protected abstract Schema Build();

		private Schema State { get; }
	}

	public abstract class Schema<T> : Schema where T : Model
	{
		public override T Mount()
		{
			T result = (T)Activator.CreateInstance(typeof(T), ToImmutable()).AssertNotNull();
			new Pipeline(result).Invoke(PipeOption.Mount);
			return result;
		}

		protected Schema(Hashtable props)
			: base(props)
		{
		}

		protected Schema(Hashtable props, Schema state)
			: base(props, state)
		{
		}
	}
}
