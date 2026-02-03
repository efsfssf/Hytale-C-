using System;
using System.IO;

namespace HytaleClient.Utils
{
	// Token: 0x020007D2 RID: 2002
	internal static class StreamExtensions
	{
		// Token: 0x06003436 RID: 13366 RVA: 0x00053C18 File Offset: 0x00051E18
		public static byte[] ReadAllBytes(this Stream reader)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] array = new byte[4096];
				int count;
				while ((count = reader.Read(array, 0, array.Length)) != 0)
				{
					memoryStream.Write(array, 0, count);
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x00053C84 File Offset: 0x00051E84
		public static void WriteInt32Be(this BinaryWriter bw, int i)
		{
			byte[] bytes = BitConverter.GetBytes(i);
			Array.Reverse(bytes);
			bw.Write(bytes);
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x00053CA8 File Offset: 0x00051EA8
		public static int ReadInt32Be(this BinaryReader br)
		{
			byte[] array = br.ReadBytes(4);
			Array.Reverse(array);
			return BitConverter.ToInt32(array, 0);
		}
	}
}
