using System;

namespace Epic.OnlineServices
{
	// Token: 0x02000016 RID: 22
	public sealed class ContinuanceToken : Handle
	{
		// Token: 0x06000315 RID: 789 RVA: 0x000044D1 File Offset: 0x000026D1
		public ContinuanceToken()
		{
		}

		// Token: 0x06000316 RID: 790 RVA: 0x000044DB File Offset: 0x000026DB
		public ContinuanceToken(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06000317 RID: 791 RVA: 0x000044E8 File Offset: 0x000026E8
		public Result ToString(out Utf8String outBuffer)
		{
			int size = 1024;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_ContinuanceToken_ToString(base.InnerHandle, intPtr, ref size);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00004528 File Offset: 0x00002728
		public override string ToString()
		{
			Utf8String u8str;
			this.ToString(out u8str);
			return u8str;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000454C File Offset: 0x0000274C
		public override string ToString(string format, IFormatProvider formatProvider)
		{
			bool flag = format != null;
			string result;
			if (flag)
			{
				result = string.Format(format, this.ToString());
			}
			else
			{
				result = this.ToString();
			}
			return result;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000457C File Offset: 0x0000277C
		public static explicit operator Utf8String(ContinuanceToken value)
		{
			Utf8String result = null;
			bool flag = value != null;
			if (flag)
			{
				value.ToString(out result);
			}
			return result;
		}
	}
}
