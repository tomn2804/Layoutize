namespace SchemataPreview
{
	public abstract class Extension<T> : Model where T : Model, new()
	{
		public override bool Exists => BaseModel.Exists;
		public override dynamic Schema { get => BaseModel.Schema; internal set => BaseModel.Schema = value; }

		public override Model? Parent { get => BaseModel.Parent; internal set => BaseModel.Parent = value; }
		public override ModelSet? Children => BaseModel.Children;

		protected T BaseModel { get; } = new();

		public override bool InvokeMethod(string name)
		{
			return BaseModel.InvokeMethod(name) && base.InvokeMethod(name);
		}
	}
}
