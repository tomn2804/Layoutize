namespace SchemataPreview
{
	public static class EventController
	{
		public static void Mount(Model model)
		{
			if (model.Exists)
			{
				if (model.ShouldHardMount)
				{
					model.EventHandler.Invoke(EventOption.Delete);
					model.EventHandler.Invoke(EventOption.Create);
				}
			}
			else
			{
				model.EventHandler.Invoke(EventOption.Create);
			}
			model.Children.ForEach(child => Mount(child));
			if (!model.IsMounted)
			{
				model.EventHandler.Invoke(EventOption.Mount);
			}
		}

		public static void Dismount(Model model)
		{
			if (model.IsMounted)
			{
				model.EventHandler.Invoke(EventOption.Dismount);
			}
			model.Children.ForEach(child => Dismount(child));
		}

		public static void Create(Model model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (!model.Exists)
			{
				model.EventHandler.Invoke(EventOption.Create);
			}
			model.Children.ForEach(child => Create(child));
		}

		public static void Delete(Model model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (model.Exists)
			{
				model.EventHandler.Invoke(EventOption.Delete);
			}
			model.Children.ForEach(child => Delete(child));
		}

		public static void Update(Model model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (model.Exists)
			{
				model.EventHandler.Invoke(EventOption.Update);
			}
			model.Children.ForEach(child => Update(child));
		}

		public static void Cleanup(Model model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (model.Exists)
			{
				model.EventHandler.Invoke(EventOption.Cleanup);
			}
			model.Children.ForEach(child => Cleanup(child));
		}
	}
}
