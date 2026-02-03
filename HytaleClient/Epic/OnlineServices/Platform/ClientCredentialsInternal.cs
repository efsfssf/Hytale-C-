using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x0200070A RID: 1802
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ClientCredentialsInternal : IGettable<ClientCredentials>, ISettable<ClientCredentials>, IDisposable
	{
		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06002E86 RID: 11910 RVA: 0x00044E48 File Offset: 0x00043048
		// (set) Token: 0x06002E87 RID: 11911 RVA: 0x00044E69 File Offset: 0x00043069
		public Utf8String ClientId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ClientId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientId);
			}
		}

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06002E88 RID: 11912 RVA: 0x00044E7C File Offset: 0x0004307C
		// (set) Token: 0x06002E89 RID: 11913 RVA: 0x00044E9D File Offset: 0x0004309D
		public Utf8String ClientSecret
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ClientSecret, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientSecret);
			}
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x00044EAD File Offset: 0x000430AD
		public void Set(ref ClientCredentials other)
		{
			this.ClientId = other.ClientId;
			this.ClientSecret = other.ClientSecret;
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x00044ECC File Offset: 0x000430CC
		public void Set(ref ClientCredentials? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientId = other.Value.ClientId;
				this.ClientSecret = other.Value.ClientSecret;
			}
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x00044F10 File Offset: 0x00043110
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientId);
			Helper.Dispose(ref this.m_ClientSecret);
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x00044F2B File Offset: 0x0004312B
		public void Get(out ClientCredentials output)
		{
			output = default(ClientCredentials);
			output.Set(ref this);
		}

		// Token: 0x04001493 RID: 5267
		private IntPtr m_ClientId;

		// Token: 0x04001494 RID: 5268
		private IntPtr m_ClientSecret;
	}
}
