using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200065E RID: 1630
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PinGrantInfoInternal : IGettable<PinGrantInfo>, ISettable<PinGrantInfo>, IDisposable
	{
		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06002A21 RID: 10785 RVA: 0x0003DC98 File Offset: 0x0003BE98
		// (set) Token: 0x06002A22 RID: 10786 RVA: 0x0003DCB9 File Offset: 0x0003BEB9
		public Utf8String UserCode
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_UserCode, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UserCode);
			}
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06002A23 RID: 10787 RVA: 0x0003DCCC File Offset: 0x0003BECC
		// (set) Token: 0x06002A24 RID: 10788 RVA: 0x0003DCED File Offset: 0x0003BEED
		public Utf8String VerificationURI
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_VerificationURI, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_VerificationURI);
			}
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06002A25 RID: 10789 RVA: 0x0003DD00 File Offset: 0x0003BF00
		// (set) Token: 0x06002A26 RID: 10790 RVA: 0x0003DD18 File Offset: 0x0003BF18
		public int ExpiresIn
		{
			get
			{
				return this.m_ExpiresIn;
			}
			set
			{
				this.m_ExpiresIn = value;
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06002A27 RID: 10791 RVA: 0x0003DD24 File Offset: 0x0003BF24
		// (set) Token: 0x06002A28 RID: 10792 RVA: 0x0003DD45 File Offset: 0x0003BF45
		public Utf8String VerificationURIComplete
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_VerificationURIComplete, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_VerificationURIComplete);
			}
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x0003DD55 File Offset: 0x0003BF55
		public void Set(ref PinGrantInfo other)
		{
			this.m_ApiVersion = 2;
			this.UserCode = other.UserCode;
			this.VerificationURI = other.VerificationURI;
			this.ExpiresIn = other.ExpiresIn;
			this.VerificationURIComplete = other.VerificationURIComplete;
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x0003DD94 File Offset: 0x0003BF94
		public void Set(ref PinGrantInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.UserCode = other.Value.UserCode;
				this.VerificationURI = other.Value.VerificationURI;
				this.ExpiresIn = other.Value.ExpiresIn;
				this.VerificationURIComplete = other.Value.VerificationURIComplete;
			}
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x0003DE09 File Offset: 0x0003C009
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserCode);
			Helper.Dispose(ref this.m_VerificationURI);
			Helper.Dispose(ref this.m_VerificationURIComplete);
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x0003DE30 File Offset: 0x0003C030
		public void Get(out PinGrantInfo output)
		{
			output = default(PinGrantInfo);
			output.Set(ref this);
		}

		// Token: 0x0400120E RID: 4622
		private int m_ApiVersion;

		// Token: 0x0400120F RID: 4623
		private IntPtr m_UserCode;

		// Token: 0x04001210 RID: 4624
		private IntPtr m_VerificationURI;

		// Token: 0x04001211 RID: 4625
		private int m_ExpiresIn;

		// Token: 0x04001212 RID: 4626
		private IntPtr m_VerificationURIComplete;
	}
}
