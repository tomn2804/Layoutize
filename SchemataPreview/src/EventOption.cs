namespace SchemataPreview
{
	public static class EventOption
	{
		public static string PostMount { get => "__PostMount"; }
		public static string PreDismount { get => "__PreDismount"; }
		public static string Create { get => "__Create"; }
		public static string Delete { get => "__Delete"; }
		public static string Update { get => "__Update"; }
		public static string Cleanup { get => "__Cleanup"; }
	}
}
