using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace HytaleClient.Utils
{
	// Token: 0x020007B4 RID: 1972
	internal static class BsonHelper
	{
		// Token: 0x06003324 RID: 13092 RVA: 0x0004DF08 File Offset: 0x0004C108
		public static sbyte[] ToBson(JToken token)
		{
			sbyte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BsonDataWriter bsonDataWriter = new BsonDataWriter(memoryStream))
				{
					token.WriteTo(bsonDataWriter, Array.Empty<JsonConverter>());
					result = (sbyte[])memoryStream.ToArray();
				}
			}
			return result;
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x0004DF74 File Offset: 0x0004C174
		public static JToken FromBson(sbyte[] bson)
		{
			JToken result;
			using (MemoryStream memoryStream = new MemoryStream((byte[])bson))
			{
				using (BsonDataReader bsonDataReader = new BsonDataReader(memoryStream))
				{
					result = JToken.ReadFrom(bsonDataReader);
				}
			}
			return result;
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x0004DFD4 File Offset: 0x0004C1D4
		public static T ObjectFromBson<T>(sbyte[] bson)
		{
			T result;
			using (MemoryStream memoryStream = new MemoryStream((byte[])bson))
			{
				using (BsonDataReader bsonDataReader = new BsonDataReader(memoryStream))
				{
					JsonSerializer jsonSerializer = new JsonSerializer();
					result = jsonSerializer.Deserialize<T>(bsonDataReader);
				}
			}
			return result;
		}
	}
}
