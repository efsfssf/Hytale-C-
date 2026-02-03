using System;

namespace Epic.OnlineServices
{
	// Token: 0x02000017 RID: 23
	public sealed class EpicAccountId : Handle
	{
		// Token: 0x0600031B RID: 795 RVA: 0x000045A7 File Offset: 0x000027A7
		public EpicAccountId()
		{
		}

		// Token: 0x0600031C RID: 796 RVA: 0x000045B1 File Offset: 0x000027B1
		public EpicAccountId(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x0600031D RID: 797 RVA: 0x000045BC File Offset: 0x000027BC
		public static EpicAccountId FromString(Utf8String accountIdString)
		{
			IntPtr zero = IntPtr.Zero;
			Helper.Set(accountIdString, ref zero);
			IntPtr from = Bindings.EOS_EpicAccountId_FromString(zero);
			Helper.Dispose(ref zero);
			EpicAccountId result;
			Helper.Get<EpicAccountId>(from, out result);
			return result;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x000045F8 File Offset: 0x000027F8
		public static explicit operator EpicAccountId(Utf8String value)
		{
			return EpicAccountId.FromString(value);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00004610 File Offset: 0x00002810
		public bool IsValid()
		{
			int from = Bindings.EOS_EpicAccountId_IsValid(base.InnerHandle);
			bool result;
			Helper.Get(from, out result);
			return result;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00004638 File Offset: 0x00002838
		public Result ToString(out Utf8String outBuffer)
		{
			int size = 33;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_EpicAccountId_ToString(base.InnerHandle, intPtr, ref size);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00004674 File Offset: 0x00002874
		public override string ToString()
		{
			Utf8String u8str;
			this.ToString(out u8str);
			return u8str;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00004698 File Offset: 0x00002898
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

		// Token: 0x06000323 RID: 803 RVA: 0x000046C8 File Offset: 0x000028C8
		public static explicit operator Utf8String(EpicAccountId value)
		{
			Utf8String result = null;
			bool flag = value != null;
			if (flag)
			{
				value.ToString(out result);
			}
			return result;
		}

		// Token: 0x04000024 RID: 36
		public const int EpicaccountidMaxLength = 32;
	}
}
