using System;
using System.Collections;

namespace SchemataPreview
{
	public class FileSchema : Schema<FileModel>
	{
		public FileSchema(Hashtable hashtable)
			: base(hashtable)
		{
		}

		protected override Schema Build()
		{
			return this;
		}
	}

	public abstract class Schema : Hashtable
	{
		public abstract Model Mount();

		public ImmutableSchema ToImmutable()
		{
			return new(State);
		}

		protected Schema(Hashtable hashtable)
			: base(hashtable)
		{
			State = Build();
		}

		protected abstract Schema Build();

		private Schema State { get; }
	}

	public abstract class Schema<T> : Schema where T : Model
	{
		public override T Mount()
		{
			return (T)Activator.CreateInstance(typeof(T), ToImmutable()).AssertNotNull();
		}

		protected Schema(Hashtable hashtable)
			: base(hashtable)
		{
		}
	}

	public class TextSchema : Schema<TextModel>
	{
		public TextSchema(Hashtable hashtable)
			: base(hashtable)
		{
		}

		protected override Schema Build()
		{
			Action<TextModel> handleOnCreated = (model) => { model.Contents = new string[] { "test" }; };
			return new FileSchema(new Hashtable() { { "Name", "test" }, { "OnCreated", handleOnCreated } });
		}
	}
}
