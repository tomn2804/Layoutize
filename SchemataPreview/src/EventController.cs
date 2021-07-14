using System.IO;

namespace SchemataPreview
{
	public static class EventController
	{
		public static void Create(Model model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (!model.Exists)
			{
				model.InvokeEvent(EventOption.Create);
			}
			foreach (Model child in model.Children)
			{
				Create(child);
			}
		}

		public static void Delete(Model model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (model.Exists)
			{
				model.InvokeEvent(EventOption.Delete);
			}
			foreach (Model child in model.Children)
			{
				Delete(child);
			}
		}

		public static void Update(Model model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (model.Exists)
			{
				model.InvokeEvent(EventOption.Update);
			}
			foreach (Model child in model.Children)
			{
				Update(child);
			}
		}

		public static void Cleanup(Model model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (model.Exists)
			{
				model.InvokeEvent(EventOption.Cleanup);
			}
			foreach (Model child in model.Children)
			{
				Cleanup(child);
			}
		}
	}
}
