namespace SchemataPreview
{
	public static class ModelBuilder
	{
		public static void HandleCreate(in Model model)
		{
			model.InvokeMethod("Create");
			model.InvokeEvent("OnCreated");
		}

		public static void HandleDelete(in Model model)
		{
			model.InvokeMethod("Delete");
			model.InvokeEvent("OnDeleted");
		}

		public static void HandleMount(in Model model)
		{
			model.Mount();
			model.InvokeEvent("OnMounted");
		}

		public static void HandleMount(in ModelSet models)
		{
			models.Mount();
			models.Parent.InvokeEvent("OnMounted");
		}
	}
}
