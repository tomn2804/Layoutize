namespace SchemataPreview
{
	public abstract class Extension<T> : Model where T : Model, new()
	{
		public override ModelSet? Children => BaseModel.Children;
		public override bool Exists => BaseModel.Exists;
		public override Model? Parent { get => BaseModel.Parent; internal set => BaseModel.Parent = value; }
		public override dynamic Schema { get => BaseModel.Schema; internal set => BaseModel.Schema = value; }
		protected T BaseModel { get; } = new();

		public override bool InvokeMethod(string name)
		{
			return BaseModel.InvokeMethod(name) && base.InvokeMethod(name);
		}
	}
}
