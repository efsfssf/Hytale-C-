using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000147 RID: 327
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendInviteOptionsInternal : ISettable<SendInviteOptions>, IDisposable
	{
		// Token: 0x1700021D RID: 541
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x0000DB58 File Offset: 0x0000BD58
		public Utf8String SessionName
		{
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x1700021E RID: 542
		// (set) Token: 0x060009E5 RID: 2533 RVA: 0x0000DB68 File Offset: 0x0000BD68
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700021F RID: 543
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x0000DB78 File Offset: 0x0000BD78
		public ProductUserId TargetUserId
		{
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0000DB88 File Offset: 0x0000BD88
		public void Set(ref SendInviteOptions other)
		{
			this.m_ApiVersion = 1;
			this.SessionName = other.SessionName;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0000DBBC File Offset: 0x0000BDBC
		public void Set(ref SendInviteOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SessionName = other.Value.SessionName;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0000DC1C File Offset: 0x0000BE1C
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionName);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x04000479 RID: 1145
		private int m_ApiVersion;

		// Token: 0x0400047A RID: 1146
		private IntPtr m_SessionName;

		// Token: 0x0400047B RID: 1147
		private IntPtr m_LocalUserId;

		// Token: 0x0400047C RID: 1148
		private IntPtr m_TargetUserId;
	}
}
