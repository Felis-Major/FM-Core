namespace FM.Runtime.Systems.Save
{
	public class BinaryDataBundle : DataBundle<byte[]>
	{
		public override void Load(string filePath)
		{

		}

		public override void Save(string filePath)
		{

		}

		protected override T DeserializeData<T>(byte[] serializedData)
		{
			return default;
		}

		protected override byte[] SerializeData<T>(T deserializedData)
		{
			return default;
		}
	}
}