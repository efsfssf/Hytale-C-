using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001D7 RID: 471
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SetRoomSettingOptionsInternal : ISettable<SetRoomSettingOptions>, IDisposable
	{
		// Token: 0x17000356 RID: 854
		// (set) Token: 0x06000DAF RID: 3503 RVA: 0x0001404B File Offset: 0x0001224B
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000357 RID: 855
		// (set) Token: 0x06000DB0 RID: 3504 RVA: 0x0001405B File Offset: 0x0001225B
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x17000358 RID: 856
		// (set) Token: 0x06000DB1 RID: 3505 RVA: 0x0001406B File Offset: 0x0001226B
		public Utf8String SettingName
		{
			set
			{
				Helper.Set(value, ref this.m_SettingName);
			}
		}

		// Token: 0x17000359 RID: 857
		// (set) Token: 0x06000DB2 RID: 3506 RVA: 0x0001407B File Offset: 0x0001227B
		public Utf8String SettingValue
		{
			set
			{
				Helper.Set(value, ref this.m_SettingValue);
			}
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0001408B File Offset: 0x0001228B
		public void Set(ref SetRoomSettingOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.SettingName = other.SettingName;
			this.SettingValue = other.SettingValue;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x000140CC File Offset: 0x000122CC
		public void Set(ref SetRoomSettingOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.SettingName = other.Value.SettingName;
				this.SettingValue = other.Value.SettingValue;
			}
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00014141 File Offset: 0x00012341
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_SettingName);
			Helper.Dispose(ref this.m_SettingValue);
		}

		// Token: 0x04000636 RID: 1590
		private int m_ApiVersion;

		// Token: 0x04000637 RID: 1591
		private IntPtr m_LocalUserId;

		// Token: 0x04000638 RID: 1592
		private IntPtr m_RoomName;

		// Token: 0x04000639 RID: 1593
		private IntPtr m_SettingName;

		// Token: 0x0400063A RID: 1594
		private IntPtr m_SettingValue;
	}
}
