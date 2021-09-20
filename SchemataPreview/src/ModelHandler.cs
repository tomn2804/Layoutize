using System;
using System.Collections.Generic;

namespace SchemataPreview
{
	public static class ModelHandler
	{
		public static void Traverse(Model model, Func<Action> func)
		{
			switch (model.Schema["Traversal"])
			{
				case "ReversePostOrder":
					TraverseReversePostOrder(model, func);
					break;

				case "ReversePreOrder":
				default:
					TraverseReversePreOrder(model, func);
					break;
			}
		}

		public static void TraverseReversePostOrder(Model model, Func<Action> func)
		{
			Action callback = func.Invoke();
			if (model.Children != null)
			{
				TraverseReversePostOrder(model.Children, func);
			}
			callback.Invoke();
		}

		public static void TraverseReversePostOrder(Model.ModelSet models, Func<Action> func)
		{
			List<Action> callbacks = new();
			foreach (Model _ in models)
			{
				callbacks.Add(func.Invoke());
			}
			foreach (Model model in models)
			{
				if (model.Children != null)
				{
					TraverseReversePostOrder(model.Children, func);
				}
				foreach (Action callback in callbacks)
				{
					callback.Invoke();
				}
			}
		}

		public static void TraverseReversePreOrder(Model model, Func<Action> func)
		{
			Action callback = func.Invoke();
			if (model.Children != null)
			{
				TraverseReversePreOrder(model.Children, func);
			}
			callback.Invoke();
		}

		public static void TraverseReversePreOrder(Model.ModelSet models, Func<Action> func)
		{
			foreach (Model model in models)
			{
				Action callback = func.Invoke();
				if (model.Children != null)
				{
					TraverseReversePreOrder(model.Children, func);
				}
				callback.Invoke();
			}
		}
	}
}
