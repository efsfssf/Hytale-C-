using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006BC RID: 1724
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogPlayerUseWeaponOptionsInternal : ISettable<LogPlayerUseWeaponOptions>, IDisposable
	{
		// Token: 0x17000D4D RID: 3405
		// (set) Token: 0x06002CAF RID: 11439 RVA: 0x00042049 File Offset: 0x00040249
		public LogPlayerUseWeaponData? UseWeaponData
		{
			set
			{
				Helper.Set<LogPlayerUseWeaponData, LogPlayerUseWeaponDataInternal>(ref value, ref this.m_UseWeaponData);
			}
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x0004205A File Offset: 0x0004025A
		public void Set(ref LogPlayerUseWeaponOptions other)
		{
			this.m_ApiVersion = 2;
			this.UseWeaponData = other.UseWeaponData;
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x00042074 File Offset: 0x00040274
		public void Set(ref LogPlayerUseWeaponOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.UseWeaponData = other.Value.UseWeaponData;
			}
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000420AA File Offset: 0x000402AA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UseWeaponData);
		}

		// Token: 0x040013AE RID: 5038
		private int m_ApiVersion;

		// Token: 0x040013AF RID: 5039
		private IntPtr m_UseWeaponData;
	}
}
