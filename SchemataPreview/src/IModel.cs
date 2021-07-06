namespace SchemataPreview
{
	public interface IModel
	{
		void OnCreate();

		void OnDelete();

		bool Exists();

		void ModelDidMount()
		{
		}

		void ModelWillDismount()
		{
		}
	}
}
