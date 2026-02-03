using System;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace HytaleClient.Utils
{
	// Token: 0x020007CB RID: 1995
	public static class ProtoHelper
	{
		// Token: 0x06003410 RID: 13328 RVA: 0x00052F70 File Offset: 0x00051170
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte RadianToByte(float rad)
		{
			return (byte)(rad * 40.425354f);
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x00052F8C File Offset: 0x0005118C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ByteToRadian(byte b)
		{
			return (float)b * 0.024639944f;
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x00052FA8 File Offset: 0x000511A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int SerializeFloat(float p)
		{
			return (int)(p * 32f);
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x00052FC4 File Offset: 0x000511C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int SerializeFloat(double p)
		{
			return (int)(p * 32.0);
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x00052FE4 File Offset: 0x000511E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DeserializeFloat(int i)
		{
			return (float)i / 32f;
		}

		// Token: 0x06003415 RID: 13333 RVA: 0x00053000 File Offset: 0x00051200
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int SerializeFloatPrecise(float p)
		{
			return (int)(p * 1000f);
		}

		// Token: 0x06003416 RID: 13334 RVA: 0x0005301C File Offset: 0x0005121C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DeserializeFloatPrecise(int i)
		{
			return (float)i / 1000f;
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x00053038 File Offset: 0x00051238
		public static JObject DeserializeBson(byte[] data)
		{
			bool flag = data.Length == 0;
			JObject result;
			if (flag)
			{
				result = null;
			}
			else
			{
				using (MemoryStream memoryStream = new MemoryStream(data))
				{
					using (BsonDataReader bsonDataReader = new BsonDataReader(memoryStream))
					{
						JsonSerializer jsonSerializer = new JsonSerializer();
						result = jsonSerializer.Deserialize<JObject>(bsonDataReader);
					}
				}
			}
			return result;
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x000530A8 File Offset: 0x000512A8
		public static byte[] SerializeBson(JObject val)
		{
			bool flag = val == null;
			byte[] result;
			if (flag)
			{
				result = new byte[0];
			}
			else
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (BsonDataWriter bsonDataWriter = new BsonDataWriter(memoryStream))
					{
						JsonSerializer jsonSerializer = new JsonSerializer();
						jsonSerializer.Serialize(bsonDataWriter, val);
						result = memoryStream.ToArray();
					}
				}
			}
			return result;
		}

		// Token: 0x04001756 RID: 5974
		private const float RbRatio = 40.425354f;

		// Token: 0x04001757 RID: 5975
		private const float RbInverse = 0.024639944f;
	}
}
