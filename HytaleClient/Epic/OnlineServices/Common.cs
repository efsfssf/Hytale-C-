using System;

namespace Epic.OnlineServices
{
	// Token: 0x02000014 RID: 20
	public sealed class Common
	{
		// Token: 0x06000310 RID: 784 RVA: 0x0000440C File Offset: 0x0000260C
		public static bool IsOperationComplete(Result result)
		{
			int from = Bindings.EOS_EResult_IsOperationComplete(result);
			bool result2;
			Helper.Get(from, out result2);
			return result2;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00004430 File Offset: 0x00002630
		public static Utf8String ToString(Result result)
		{
			IntPtr from = Bindings.EOS_EResult_ToString(result);
			Utf8String result2;
			Helper.Get(from, out result2);
			return result2;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00004454 File Offset: 0x00002654
		public static Result ToString(ArraySegment<byte> byteArray, out Utf8String outBuffer)
		{
			IntPtr zero = IntPtr.Zero;
			uint length;
			Helper.Set(byteArray, ref zero, out length);
			uint size = 1024U;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_ByteArray_ToString(zero, length, intPtr, ref size);
			Helper.Dispose(ref zero);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x000044AC File Offset: 0x000026AC
		public static Utf8String ToString(ArraySegment<byte> byteArray)
		{
			Utf8String result;
			Common.ToString(byteArray, out result);
			return result;
		}

		// Token: 0x04000012 RID: 18
		public const ulong InvalidNotificationid = 0UL;

		// Token: 0x04000013 RID: 19
		public const int PagequeryApiLatest = 1;

		// Token: 0x04000014 RID: 20
		public const int PagequeryMaxcountDefault = 10;

		// Token: 0x04000015 RID: 21
		public const int PagequeryMaxcountMaximum = 100;

		// Token: 0x04000016 RID: 22
		public const int PaginationApiLatest = 1;
	}
}
