using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000624 RID: 1572
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UserLoginInfoInternal : IGettable<UserLoginInfo>, ISettable<UserLoginInfo>, IDisposable
	{
		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x060028A0 RID: 10400 RVA: 0x0003B898 File Offset: 0x00039A98
		// (set) Token: 0x060028A1 RID: 10401 RVA: 0x0003B8B9 File Offset: 0x00039AB9
		public Utf8String DisplayName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DisplayName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DisplayName);
			}
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x060028A2 RID: 10402 RVA: 0x0003B8CC File Offset: 0x00039ACC
		// (set) Token: 0x060028A3 RID: 10403 RVA: 0x0003B8ED File Offset: 0x00039AED
		public Utf8String NsaIdToken
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_NsaIdToken, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_NsaIdToken);
			}
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x0003B8FD File Offset: 0x00039AFD
		public void Set(ref UserLoginInfo other)
		{
			this.m_ApiVersion = 2;
			this.DisplayName = other.DisplayName;
			this.NsaIdToken = other.NsaIdToken;
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x0003B924 File Offset: 0x00039B24
		public void Set(ref UserLoginInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.DisplayName = other.Value.DisplayName;
				this.NsaIdToken = other.Value.NsaIdToken;
			}
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x0003B96F File Offset: 0x00039B6F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_DisplayName);
			Helper.Dispose(ref this.m_NsaIdToken);
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x0003B98A File Offset: 0x00039B8A
		public void Get(out UserLoginInfo output)
		{
			output = default(UserLoginInfo);
			output.Set(ref this);
		}

		// Token: 0x0400116C RID: 4460
		private int m_ApiVersion;

		// Token: 0x0400116D RID: 4461
		private IntPtr m_DisplayName;

		// Token: 0x0400116E RID: 4462
		private IntPtr m_NsaIdToken;
	}
}
