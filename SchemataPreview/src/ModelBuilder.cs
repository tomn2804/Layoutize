namespace SchemataPreview
{
	public static class ModelBuilder
	{
		public static void HandleCreate(in Model model)
		{
			model.InvokeMethod(MethodOption.Create);
			model.InvokeEvent(EventOption.OnCreated);
		}

		public static void HandleDelete(in Model model)
		{
			model.InvokeMethod(MethodOption.Delete);
			model.InvokeEvent(EventOption.OnDeleted);
		}

		public static void HandleMount(in Model model)
		{
			model.Mount();
			model.InvokeEvent(EventOption.OnMounted);
		}

		public static void HandleMount(in ModelSet models)
		{
			models.Mount();
			models.Parent.InvokeEvent(EventOption.OnMounted);
		}
	}
}
