using System.Collections.Generic;
using System.Management.Automation;
using System.Reflection;

namespace SchemataPreview
{
	public class Model
	{
		public virtual Schema Schema { get; internal set; }

		public virtual void InvokeMethod(string name)
		{
			if (GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic) is var method && method != null)
			{
				method.Invoke(this, null);
			}
			if (Schema.TryGetValue(name, out object callback))
			{
				((ScriptBlock)callback).InvokeWithContext(null, new List<PSVariable> { new PSVariable("this", this) });
			}
		}

		public static implicit operator string(Model rhs)
		{
			return Schema["FullName"];
		}
	}

	public class Model<T> : Model where T : Model, new()
	{
		protected T BaseModel { get; } = new();

		public override Schema Schema
		{
			get => base.Schema;
			internal set
			{
				BaseModel.Schema = value;
				base.Schema = value;
			}
		}

		public override void InvokeMethod(string name)
		{
			BaseModel.InvokeMethod(name);
			base.InvokeMethod(name);
		}
	}
}
