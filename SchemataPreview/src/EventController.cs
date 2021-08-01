using System.IO;

namespace SchemataPreview
{
	public static class EventController
	{
		public static void Create(Schema model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (!model.Exists)
			{
				model.InvokeEvent(EventOption.Create);
			}
			foreach (Schema child in model.Children)
			{
				Create(child);
			}
		}

		public static void Delete(Schema model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (model.Exists)
			{
				model.InvokeEvent(EventOption.Delete);
			}
			foreach (Schema child in model.Children)
			{
				Delete(child);
			}
		}

		public static void Update(Schema model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (model.Exists)
			{
				model.InvokeEvent(EventOption.Update);
			}
			foreach (Schema child in model.Children)
			{
				Update(child);
			}
		}

		public static void Cleanup(Schema model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (model.Exists)
			{
				model.InvokeEvent(EventOption.Cleanup);
			}
			foreach (Schema child in model.Children)
			{
				Cleanup(child);
			}
		}
	}
}
