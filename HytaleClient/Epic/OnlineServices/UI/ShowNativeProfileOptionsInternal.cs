using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000091 RID: 145
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ShowNativeProfileOptionsInternal : ISettable<ShowNativeProfileOptions>, IDisposable
	{
		// Token: 0x170000F4 RID: 244
		// (set) Token: 0x060005DC RID: 1500 RVA: 0x00008325 File Offset: 0x00006525
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170000F5 RID: 245
		// (set) Token: 0x060005DD RID: 1501 RVA: 0x00008335 File Offset: 0x00006535
		public EpicAccountId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00008345 File Offset: 0x00006545
		public void Set(ref ShowNativeProfileOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0000836C File Offset: 0x0000656C
		public void Set(ref ShowNativeProfileOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000083B7 File Offset: 0x000065B7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x040002DA RID: 730
		private int m_ApiVersion;

		// Token: 0x040002DB RID: 731
		private IntPtr m_LocalUserId;

		// Token: 0x040002DC RID: 732
		private IntPtr m_TargetUserId;
	}
}
