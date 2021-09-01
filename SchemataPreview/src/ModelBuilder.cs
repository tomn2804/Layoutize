namespace SchemataPreview
{
	public static class ModelBuilder
	{
		public static void HandleCreate(in Model model)
		{
			model.InvokeMethod(nameof(MethodOption.Mount));
			model.InvokeEvent(nameof(EventOption.OnCreated));
		}

		public static void HandleDelete(in Model model)
		{
			model.InvokeMethod(nameof(MethodOption.Delete));
			model.InvokeEvent(nameof(EventOption.OnDeleted));
		}

		public static void HandleMount(in Model model)
		{
			model.Mount();
			model.InvokeEvent(nameof(EventOption.OnMounted));
		}

		public static void HandleMount(in ModelSet models)
		{
			models.Mount();
			models.Parent.InvokeEvent(nameof(EventOption.OnMounted));
		}
	}
}
