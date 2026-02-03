using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x0200068D RID: 1677
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RegisterClientOptionsInternal : ISettable<RegisterClientOptions>, IDisposable
	{
		// Token: 0x17000CBE RID: 3262
		// (set) Token: 0x06002B8A RID: 11146 RVA: 0x00040141 File Offset: 0x0003E341
		public IntPtr ClientHandle
		{
			set
			{
				this.m_ClientHandle = value;
			}
		}

		// Token: 0x17000CBF RID: 3263
		// (set) Token: 0x06002B8B RID: 11147 RVA: 0x0004014B File Offset: 0x0003E34B
		public AntiCheatCommonClientType ClientType
		{
			set
			{
				this.m_ClientType = value;
			}
		}

		// Token: 0x17000CC0 RID: 3264
		// (set) Token: 0x06002B8C RID: 11148 RVA: 0x00040155 File Offset: 0x0003E355
		public AntiCheatCommonClientPlatform ClientPlatform
		{
			set
			{
				this.m_ClientPlatform = value;
			}
		}

		// Token: 0x17000CC1 RID: 3265
		// (set) Token: 0x06002B8D RID: 11149 RVA: 0x0004015F File Offset: 0x0003E35F
		public Utf8String AccountId_DEPRECATED
		{
			set
			{
				Helper.Set(value, ref this.m_AccountId_DEPRECATED);
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (set) Token: 0x06002B8E RID: 11150 RVA: 0x0004016F File Offset: 0x0003E36F
		public Utf8String IpAddress
		{
			set
			{
				Helper.Set(value, ref this.m_IpAddress);
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (set) Token: 0x06002B8F RID: 11151 RVA: 0x0004017F File Offset: 0x0003E37F
		public ProductUserId UserId
		{
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (set) Token: 0x06002B90 RID: 11152 RVA: 0x0004018F File Offset: 0x0003E38F
		public int Reserved01
		{
			set
			{
				this.m_Reserved01 = value;
			}
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x0004019C File Offset: 0x0003E39C
		public void Set(ref RegisterClientOptions other)
		{
			this.m_ApiVersion = 3;
			this.ClientHandle = other.ClientHandle;
			this.ClientType = other.ClientType;
			this.ClientPlatform = other.ClientPlatform;
			this.AccountId_DEPRECATED = other.AccountId_DEPRECATED;
			this.IpAddress = other.IpAddress;
			this.UserId = other.UserId;
			this.Reserved01 = other.Reserved01;
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x0004020C File Offset: 0x0003E40C
		public void Set(ref RegisterClientOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.ClientHandle = other.Value.ClientHandle;
				this.ClientType = other.Value.ClientType;
				this.ClientPlatform = other.Value.ClientPlatform;
				this.AccountId_DEPRECATED = other.Value.AccountId_DEPRECATED;
				this.IpAddress = other.Value.IpAddress;
				this.UserId = other.Value.UserId;
				this.Reserved01 = other.Value.Reserved01;
			}
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x000402C3 File Offset: 0x0003E4C3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientHandle);
			Helper.Dispose(ref this.m_AccountId_DEPRECATED);
			Helper.Dispose(ref this.m_IpAddress);
			Helper.Dispose(ref this.m_UserId);
		}

		// Token: 0x040012A7 RID: 4775
		private int m_ApiVersion;

		// Token: 0x040012A8 RID: 4776
		private IntPtr m_ClientHandle;

		// Token: 0x040012A9 RID: 4777
		private AntiCheatCommonClientType m_ClientType;

		// Token: 0x040012AA RID: 4778
		private AntiCheatCommonClientPlatform m_ClientPlatform;

		// Token: 0x040012AB RID: 4779
		private IntPtr m_AccountId_DEPRECATED;

		// Token: 0x040012AC RID: 4780
		private IntPtr m_IpAddress;

		// Token: 0x040012AD RID: 4781
		private IntPtr m_UserId;

		// Token: 0x040012AE RID: 4782
		private int m_Reserved01;
	}
}
