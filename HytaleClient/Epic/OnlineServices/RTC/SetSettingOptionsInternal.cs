using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001D9 RID: 473
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetSettingOptionsInternal : ISettable<SetSettingOptions>, IDisposable
	{
		// Token: 0x1700035C RID: 860
		// (set) Token: 0x06000DBA RID: 3514 RVA: 0x00014196 File Offset: 0x00012396
		public Utf8String SettingName
		{
			set
			{
				Helper.Set(value, ref this.m_SettingName);
			}
		}

		// Token: 0x1700035D RID: 861
		// (set) Token: 0x06000DBB RID: 3515 RVA: 0x000141A6 File Offset: 0x000123A6
		public Utf8String SettingValue
		{
			set
			{
				Helper.Set(value, ref this.m_SettingValue);
			}
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x000141B6 File Offset: 0x000123B6
		public void Set(ref SetSettingOptions other)
		{
			this.m_ApiVersion = 1;
			this.SettingName = other.SettingName;
			this.SettingValue = other.SettingValue;
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x000141DC File Offset: 0x000123DC
		public void Set(ref SetSettingOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SettingName = other.Value.SettingName;
				this.SettingValue = other.Value.SettingValue;
			}
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00014227 File Offset: 0x00012427
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SettingName);
			Helper.Dispose(ref this.m_SettingValue);
		}

		// Token: 0x0400063D RID: 1597
		private int m_ApiVersion;

		// Token: 0x0400063E RID: 1598
		private IntPtr m_SettingName;

		// Token: 0x0400063F RID: 1599
		private IntPtr m_SettingValue;
	}
}
