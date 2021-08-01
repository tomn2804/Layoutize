namespace SchemataPreview
{
	public class StrictTextModel : Model<TextModel>
	{
		protected void OnCleanup()
		{
			BaseModel.Contents = TextEditor.Format(BaseModel.Contents);
		}
	}
}
