namespace SchemataPreview
{
	public interface IModel
	{
		void Create();

		void Delete();

		bool Exists();

		void ModelDidMount()
		{
		}

		void ModelWillDismount()
		{
		}
	}
}
