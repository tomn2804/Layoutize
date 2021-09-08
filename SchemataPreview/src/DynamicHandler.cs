using System;
using System.Diagnostics;
using System.Reflection;

namespace SchemataPreview
{
	public abstract class DynamicHandler
	{
		public bool InvokeCallback(EventOption option)
		{
			string? name = Enum.GetName(option);
			Debug.Assert(name != null);
			return InvokeCallback(name);
		}

		public abstract bool InvokeCallback(string name);

		public bool InvokeMethod(MethodOption option)
		{
			string? name = Enum.GetName(option);
			Debug.Assert(name != null);
			return InvokeMethod(name);
		}

		public bool InvokeMethod(string name)
		{
			MethodInfo? method = GetType().GetMethod(name);
			if (method == null)
			{
				return false;
			}
			method.Invoke(this, null);
			return true;
		}
	}
}
