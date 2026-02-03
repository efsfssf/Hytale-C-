using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006FB RID: 1787
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RegisterPeerOptionsInternal : ISettable<RegisterPeerOptions>, IDisposable
	{
		// Token: 0x17000DB0 RID: 3504
		// (set) Token: 0x06002E02 RID: 11778 RVA: 0x00043DC8 File Offset: 0x00041FC8
		public IntPtr PeerHandle
		{
			set
			{
				this.m_PeerHandle = value;
			}
		}

		// Token: 0x17000DB1 RID: 3505
		// (set) Token: 0x06002E03 RID: 11779 RVA: 0x00043DD2 File Offset: 0x00041FD2
		public AntiCheatCommonClientType ClientType
		{
			set
			{
				this.m_ClientType = value;
			}
		}

		// Token: 0x17000DB2 RID: 3506
		// (set) Token: 0x06002E04 RID: 11780 RVA: 0x00043DDC File Offset: 0x00041FDC
		public AntiCheatCommonClientPlatform ClientPlatform
		{
			set
			{
				this.m_ClientPlatform = value;
			}
		}

		// Token: 0x17000DB3 RID: 3507
		// (set) Token: 0x06002E05 RID: 11781 RVA: 0x00043DE6 File Offset: 0x00041FE6
		public uint AuthenticationTimeout
		{
			set
			{
				this.m_AuthenticationTimeout = value;
			}
		}

		// Token: 0x17000DB4 RID: 3508
		// (set) Token: 0x06002E06 RID: 11782 RVA: 0x00043DF0 File Offset: 0x00041FF0
		public Utf8String AccountId_DEPRECATED
		{
			set
			{
				Helper.Set(value, ref this.m_AccountId_DEPRECATED);
			}
		}

		// Token: 0x17000DB5 RID: 3509
		// (set) Token: 0x06002E07 RID: 11783 RVA: 0x00043E00 File Offset: 0x00042000
		public Utf8String IpAddress
		{
			set
			{
				Helper.Set(value, ref this.m_IpAddress);
			}
		}

		// Token: 0x17000DB6 RID: 3510
		// (set) Token: 0x06002E08 RID: 11784 RVA: 0x00043E10 File Offset: 0x00042010
		public ProductUserId PeerProductUserId
		{
			set
			{
				Helper.Set(value, ref this.m_PeerProductUserId);
			}
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x00043E20 File Offset: 0x00042020
		public void Set(ref RegisterPeerOptions other)
		{
			this.m_ApiVersion = 3;
			this.PeerHandle = other.PeerHandle;
			this.ClientType = other.ClientType;
			this.ClientPlatform = other.ClientPlatform;
			this.AuthenticationTimeout = other.AuthenticationTimeout;
			this.AccountId_DEPRECATED = other.AccountId_DEPRECATED;
			this.IpAddress = other.IpAddress;
			this.PeerProductUserId = other.PeerProductUserId;
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x00043E90 File Offset: 0x00042090
		public void Set(ref RegisterPeerOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.PeerHandle = other.Value.PeerHandle;
				this.ClientType = other.Value.ClientType;
				this.ClientPlatform = other.Value.ClientPlatform;
				this.AuthenticationTimeout = other.Value.AuthenticationTimeout;
				this.AccountId_DEPRECATED = other.Value.AccountId_DEPRECATED;
				this.IpAddress = other.Value.IpAddress;
				this.PeerProductUserId = other.Value.PeerProductUserId;
			}
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x00043F47 File Offset: 0x00042147
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PeerHandle);
			Helper.Dispose(ref this.m_AccountId_DEPRECATED);
			Helper.Dispose(ref this.m_IpAddress);
			Helper.Dispose(ref this.m_PeerProductUserId);
		}

		// Token: 0x0400144E RID: 5198
		private int m_ApiVersion;

		// Token: 0x0400144F RID: 5199
		private IntPtr m_PeerHandle;

		// Token: 0x04001450 RID: 5200
		private AntiCheatCommonClientType m_ClientType;

		// Token: 0x04001451 RID: 5201
		private AntiCheatCommonClientPlatform m_ClientPlatform;

		// Token: 0x04001452 RID: 5202
		private uint m_AuthenticationTimeout;

		// Token: 0x04001453 RID: 5203
		private IntPtr m_AccountId_DEPRECATED;

		// Token: 0x04001454 RID: 5204
		private IntPtr m_IpAddress;

		// Token: 0x04001455 RID: 5205
		private IntPtr m_PeerProductUserId;
	}
}
