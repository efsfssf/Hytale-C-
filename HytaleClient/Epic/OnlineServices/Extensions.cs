using System;

namespace Epic.OnlineServices
{
	// Token: 0x02000004 RID: 4
	public static class Extensions
	{
		// Token: 0x0600006E RID: 110 RVA: 0x00003E04 File Offset: 0x00002004
		public static bool IsOperationComplete(this Result result)
		{
			return Common.IsOperationComplete(result);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003E1C File Offset: 0x0000201C
		public static string ToHexString(this byte[] byteArray)
		{
			ArraySegment<byte> byteArray2 = new ArraySegment<byte>(byteArray);
			return Common.ToString(byteArray2);
		}
	}
}
