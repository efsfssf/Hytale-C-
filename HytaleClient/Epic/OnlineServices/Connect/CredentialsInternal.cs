using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005DC RID: 1500
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CredentialsInternal : IGettable<Credentials>, ISettable<Credentials>, IDisposable
	{
		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x060026F3 RID: 9971 RVA: 0x00039A2C File Offset: 0x00037C2C
		// (set) Token: 0x060026F4 RID: 9972 RVA: 0x00039A4D File Offset: 0x00037C4D
		public Utf8String Token
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Token, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Token);
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x060026F5 RID: 9973 RVA: 0x00039A60 File Offset: 0x00037C60
		// (set) Token: 0x060026F6 RID: 9974 RVA: 0x00039A78 File Offset: 0x00037C78
		public ExternalCredentialType Type
		{
			get
			{
				return this.m_Type;
			}
			set
			{
				this.m_Type = value;
			}
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x00039A82 File Offset: 0x00037C82
		public void Set(ref Credentials other)
		{
			this.m_ApiVersion = 1;
			this.Token = other.Token;
			this.Type = other.Type;
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x00039AA8 File Offset: 0x00037CA8
		public void Set(ref Credentials? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Token = other.Value.Token;
				this.Type = other.Value.Type;
			}
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x00039AF3 File Offset: 0x00037CF3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Token);
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x00039B02 File Offset: 0x00037D02
		public void Get(out Credentials output)
		{
			output = default(Credentials);
			output.Set(ref this);
		}

		// Token: 0x040010E6 RID: 4326
		private int m_ApiVersion;

		// Token: 0x040010E7 RID: 4327
		private IntPtr m_Token;

		// Token: 0x040010E8 RID: 4328
		private ExternalCredentialType m_Type;
	}
}
