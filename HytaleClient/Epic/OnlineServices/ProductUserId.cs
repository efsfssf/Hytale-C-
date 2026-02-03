using System;

namespace Epic.OnlineServices
{
	// Token: 0x02000020 RID: 32
	public sealed class ProductUserId : Handle
	{
		// Token: 0x06000343 RID: 835 RVA: 0x00004962 File Offset: 0x00002B62
		public ProductUserId()
		{
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000496C File Offset: 0x00002B6C
		public ProductUserId(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00004978 File Offset: 0x00002B78
		public static ProductUserId FromString(Utf8String productUserIdString)
		{
			IntPtr zero = IntPtr.Zero;
			Helper.Set(productUserIdString, ref zero);
			IntPtr from = Bindings.EOS_ProductUserId_FromString(zero);
			Helper.Dispose(ref zero);
			ProductUserId result;
			Helper.Get<ProductUserId>(from, out result);
			return result;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x000049B4 File Offset: 0x00002BB4
		public static explicit operator ProductUserId(Utf8String value)
		{
			return ProductUserId.FromString(value);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x000049CC File Offset: 0x00002BCC
		public bool IsValid()
		{
			int from = Bindings.EOS_ProductUserId_IsValid(base.InnerHandle);
			bool result;
			Helper.Get(from, out result);
			return result;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x000049F4 File Offset: 0x00002BF4
		public Result ToString(out Utf8String outBuffer)
		{
			int size = 33;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_ProductUserId_ToString(base.InnerHandle, intPtr, ref size);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00004A30 File Offset: 0x00002C30
		public override string ToString()
		{
			Utf8String u8str;
			this.ToString(out u8str);
			return u8str;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00004A54 File Offset: 0x00002C54
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

		// Token: 0x0600034B RID: 843 RVA: 0x00004A84 File Offset: 0x00002C84
		public static explicit operator Utf8String(ProductUserId value)
		{
			Utf8String result = null;
			bool flag = value != null;
			if (flag)
			{
				value.ToString(out result);
			}
			return result;
		}

		// Token: 0x04000059 RID: 89
		public const int ProductuseridMaxLength = 32;
	}
}
