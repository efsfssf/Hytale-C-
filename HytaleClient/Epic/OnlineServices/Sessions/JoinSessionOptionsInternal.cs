using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000113 RID: 275
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct JoinSessionOptionsInternal : ISettable<JoinSessionOptions>, IDisposable
	{
		// Token: 0x170001EB RID: 491
		// (set) Token: 0x060008E4 RID: 2276 RVA: 0x0000CF25 File Offset: 0x0000B125
		public Utf8String SessionName
		{
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x170001EC RID: 492
		// (set) Token: 0x060008E5 RID: 2277 RVA: 0x0000CF35 File Offset: 0x0000B135
		public SessionDetails SessionHandle
		{
			set
			{
				Helper.Set(value, ref this.m_SessionHandle);
			}
		}

		// Token: 0x170001ED RID: 493
		// (set) Token: 0x060008E6 RID: 2278 RVA: 0x0000CF45 File Offset: 0x0000B145
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170001EE RID: 494
		// (set) Token: 0x060008E7 RID: 2279 RVA: 0x0000CF55 File Offset: 0x0000B155
		public bool PresenceEnabled
		{
			set
			{
				Helper.Set(value, ref this.m_PresenceEnabled);
			}
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0000CF65 File Offset: 0x0000B165
		public void Set(ref JoinSessionOptions other)
		{
			this.m_ApiVersion = 2;
			this.SessionName = other.SessionName;
			this.SessionHandle = other.SessionHandle;
			this.LocalUserId = other.LocalUserId;
			this.PresenceEnabled = other.PresenceEnabled;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0000CFA4 File Offset: 0x0000B1A4
		public void Set(ref JoinSessionOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.SessionName = other.Value.SessionName;
				this.SessionHandle = other.Value.SessionHandle;
				this.LocalUserId = other.Value.LocalUserId;
				this.PresenceEnabled = other.Value.PresenceEnabled;
			}
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0000D019 File Offset: 0x0000B219
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionName);
			Helper.Dispose(ref this.m_SessionHandle);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x04000438 RID: 1080
		private int m_ApiVersion;

		// Token: 0x04000439 RID: 1081
		private IntPtr m_SessionName;

		// Token: 0x0400043A RID: 1082
		private IntPtr m_SessionHandle;

		// Token: 0x0400043B RID: 1083
		private IntPtr m_LocalUserId;

		// Token: 0x0400043C RID: 1084
		private int m_PresenceEnabled;
	}
}
