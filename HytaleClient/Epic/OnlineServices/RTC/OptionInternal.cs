using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001CD RID: 461
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OptionInternal : IGettable<Option>, ISettable<Option>, IDisposable
	{
		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00013300 File Offset: 0x00011500
		// (set) Token: 0x06000D44 RID: 3396 RVA: 0x00013321 File Offset: 0x00011521
		public Utf8String Key
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Key, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Key);
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x00013334 File Offset: 0x00011534
		// (set) Token: 0x06000D46 RID: 3398 RVA: 0x00013355 File Offset: 0x00011555
		public Utf8String Value
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Value, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Value);
			}
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00013365 File Offset: 0x00011565
		public void Set(ref Option other)
		{
			this.m_ApiVersion = 1;
			this.Key = other.Key;
			this.Value = other.Value;
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0001338C File Offset: 0x0001158C
		public void Set(ref Option? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Key = other.Value.Key;
				this.Value = other.Value.Value;
			}
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x000133D7 File Offset: 0x000115D7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Key);
			Helper.Dispose(ref this.m_Value);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x000133F2 File Offset: 0x000115F2
		public void Get(out Option output)
		{
			output = default(Option);
			output.Set(ref this);
		}

		// Token: 0x04000602 RID: 1538
		private int m_ApiVersion;

		// Token: 0x04000603 RID: 1539
		private IntPtr m_Key;

		// Token: 0x04000604 RID: 1540
		private IntPtr m_Value;
	}
}
