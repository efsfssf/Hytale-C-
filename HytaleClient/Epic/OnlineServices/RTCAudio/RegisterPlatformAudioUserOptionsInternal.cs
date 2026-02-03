using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200025B RID: 603
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RegisterPlatformAudioUserOptionsInternal : ISettable<RegisterPlatformAudioUserOptions>, IDisposable
	{
		// Token: 0x17000441 RID: 1089
		// (set) Token: 0x060010C5 RID: 4293 RVA: 0x00017E59 File Offset: 0x00016059
		public Utf8String UserId
		{
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x00017E69 File Offset: 0x00016069
		public void Set(ref RegisterPlatformAudioUserOptions other)
		{
			this.m_ApiVersion = 1;
			this.UserId = other.UserId;
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x00017E80 File Offset: 0x00016080
		public void Set(ref RegisterPlatformAudioUserOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.UserId = other.Value.UserId;
			}
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x00017EB6 File Offset: 0x000160B6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
		}

		// Token: 0x0400073A RID: 1850
		private int m_ApiVersion;

		// Token: 0x0400073B RID: 1851
		private IntPtr m_UserId;
	}
}
