using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000FB RID: 251
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreateSessionModificationOptionsInternal : ISettable<CreateSessionModificationOptions>, IDisposable
	{
		// Token: 0x170001B7 RID: 439
		// (set) Token: 0x06000861 RID: 2145 RVA: 0x0000C311 File Offset: 0x0000A511
		public Utf8String SessionName
		{
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x170001B8 RID: 440
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x0000C321 File Offset: 0x0000A521
		public Utf8String BucketId
		{
			set
			{
				Helper.Set(value, ref this.m_BucketId);
			}
		}

		// Token: 0x170001B9 RID: 441
		// (set) Token: 0x06000863 RID: 2147 RVA: 0x0000C331 File Offset: 0x0000A531
		public uint MaxPlayers
		{
			set
			{
				this.m_MaxPlayers = value;
			}
		}

		// Token: 0x170001BA RID: 442
		// (set) Token: 0x06000864 RID: 2148 RVA: 0x0000C33B File Offset: 0x0000A53B
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170001BB RID: 443
		// (set) Token: 0x06000865 RID: 2149 RVA: 0x0000C34B File Offset: 0x0000A54B
		public bool PresenceEnabled
		{
			set
			{
				Helper.Set(value, ref this.m_PresenceEnabled);
			}
		}

		// Token: 0x170001BC RID: 444
		// (set) Token: 0x06000866 RID: 2150 RVA: 0x0000C35B File Offset: 0x0000A55B
		public Utf8String SessionId
		{
			set
			{
				Helper.Set(value, ref this.m_SessionId);
			}
		}

		// Token: 0x170001BD RID: 445
		// (set) Token: 0x06000867 RID: 2151 RVA: 0x0000C36B File Offset: 0x0000A56B
		public bool SanctionsEnabled
		{
			set
			{
				Helper.Set(value, ref this.m_SanctionsEnabled);
			}
		}

		// Token: 0x170001BE RID: 446
		// (set) Token: 0x06000868 RID: 2152 RVA: 0x0000C37B File Offset: 0x0000A57B
		public uint[] AllowedPlatformIds
		{
			set
			{
				Helper.Set<uint>(value, ref this.m_AllowedPlatformIds, out this.m_AllowedPlatformIdsCount);
			}
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0000C394 File Offset: 0x0000A594
		public void Set(ref CreateSessionModificationOptions other)
		{
			this.m_ApiVersion = 5;
			this.SessionName = other.SessionName;
			this.BucketId = other.BucketId;
			this.MaxPlayers = other.MaxPlayers;
			this.LocalUserId = other.LocalUserId;
			this.PresenceEnabled = other.PresenceEnabled;
			this.SessionId = other.SessionId;
			this.SanctionsEnabled = other.SanctionsEnabled;
			this.AllowedPlatformIds = other.AllowedPlatformIds;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0000C414 File Offset: 0x0000A614
		public void Set(ref CreateSessionModificationOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 5;
				this.SessionName = other.Value.SessionName;
				this.BucketId = other.Value.BucketId;
				this.MaxPlayers = other.Value.MaxPlayers;
				this.LocalUserId = other.Value.LocalUserId;
				this.PresenceEnabled = other.Value.PresenceEnabled;
				this.SessionId = other.Value.SessionId;
				this.SanctionsEnabled = other.Value.SanctionsEnabled;
				this.AllowedPlatformIds = other.Value.AllowedPlatformIds;
			}
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0000C4E0 File Offset: 0x0000A6E0
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionName);
			Helper.Dispose(ref this.m_BucketId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_SessionId);
			Helper.Dispose(ref this.m_AllowedPlatformIds);
		}

		// Token: 0x040003FF RID: 1023
		private int m_ApiVersion;

		// Token: 0x04000400 RID: 1024
		private IntPtr m_SessionName;

		// Token: 0x04000401 RID: 1025
		private IntPtr m_BucketId;

		// Token: 0x04000402 RID: 1026
		private uint m_MaxPlayers;

		// Token: 0x04000403 RID: 1027
		private IntPtr m_LocalUserId;

		// Token: 0x04000404 RID: 1028
		private int m_PresenceEnabled;

		// Token: 0x04000405 RID: 1029
		private IntPtr m_SessionId;

		// Token: 0x04000406 RID: 1030
		private int m_SanctionsEnabled;

		// Token: 0x04000407 RID: 1031
		private IntPtr m_AllowedPlatformIds;

		// Token: 0x04000408 RID: 1032
		private uint m_AllowedPlatformIdsCount;
	}
}
