using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000664 RID: 1636
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct TokenInternal : IGettable<Token>, ISettable<Token>, IDisposable
	{
		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x06002A62 RID: 10850 RVA: 0x0003E298 File Offset: 0x0003C498
		// (set) Token: 0x06002A63 RID: 10851 RVA: 0x0003E2B9 File Offset: 0x0003C4B9
		public Utf8String App
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_App, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_App);
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x06002A64 RID: 10852 RVA: 0x0003E2CC File Offset: 0x0003C4CC
		// (set) Token: 0x06002A65 RID: 10853 RVA: 0x0003E2ED File Offset: 0x0003C4ED
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

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x06002A66 RID: 10854 RVA: 0x0003E300 File Offset: 0x0003C500
		// (set) Token: 0x06002A67 RID: 10855 RVA: 0x0003E321 File Offset: 0x0003C521
		public EpicAccountId AccountId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_AccountId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_AccountId);
			}
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x06002A68 RID: 10856 RVA: 0x0003E334 File Offset: 0x0003C534
		// (set) Token: 0x06002A69 RID: 10857 RVA: 0x0003E355 File Offset: 0x0003C555
		public Utf8String AccessToken
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_AccessToken, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_AccessToken);
			}
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x06002A6A RID: 10858 RVA: 0x0003E368 File Offset: 0x0003C568
		// (set) Token: 0x06002A6B RID: 10859 RVA: 0x0003E380 File Offset: 0x0003C580
		public double ExpiresIn
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

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x06002A6C RID: 10860 RVA: 0x0003E38C File Offset: 0x0003C58C
		// (set) Token: 0x06002A6D RID: 10861 RVA: 0x0003E3AD File Offset: 0x0003C5AD
		public Utf8String ExpiresAt
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ExpiresAt, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ExpiresAt);
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x06002A6E RID: 10862 RVA: 0x0003E3C0 File Offset: 0x0003C5C0
		// (set) Token: 0x06002A6F RID: 10863 RVA: 0x0003E3D8 File Offset: 0x0003C5D8
		public AuthTokenType AuthType
		{
			get
			{
				return this.m_AuthType;
			}
			set
			{
				this.m_AuthType = value;
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x06002A70 RID: 10864 RVA: 0x0003E3E4 File Offset: 0x0003C5E4
		// (set) Token: 0x06002A71 RID: 10865 RVA: 0x0003E405 File Offset: 0x0003C605
		public Utf8String RefreshToken
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_RefreshToken, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_RefreshToken);
			}
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x06002A72 RID: 10866 RVA: 0x0003E418 File Offset: 0x0003C618
		// (set) Token: 0x06002A73 RID: 10867 RVA: 0x0003E430 File Offset: 0x0003C630
		public double RefreshExpiresIn
		{
			get
			{
				return this.m_RefreshExpiresIn;
			}
			set
			{
				this.m_RefreshExpiresIn = value;
			}
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06002A74 RID: 10868 RVA: 0x0003E43C File Offset: 0x0003C63C
		// (set) Token: 0x06002A75 RID: 10869 RVA: 0x0003E45D File Offset: 0x0003C65D
		public Utf8String RefreshExpiresAt
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_RefreshExpiresAt, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_RefreshExpiresAt);
			}
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x0003E470 File Offset: 0x0003C670
		public void Set(ref Token other)
		{
			this.m_ApiVersion = 2;
			this.App = other.App;
			this.ClientId = other.ClientId;
			this.AccountId = other.AccountId;
			this.AccessToken = other.AccessToken;
			this.ExpiresIn = other.ExpiresIn;
			this.ExpiresAt = other.ExpiresAt;
			this.AuthType = other.AuthType;
			this.RefreshToken = other.RefreshToken;
			this.RefreshExpiresIn = other.RefreshExpiresIn;
			this.RefreshExpiresAt = other.RefreshExpiresAt;
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x0003E508 File Offset: 0x0003C708
		public void Set(ref Token? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.App = other.Value.App;
				this.ClientId = other.Value.ClientId;
				this.AccountId = other.Value.AccountId;
				this.AccessToken = other.Value.AccessToken;
				this.ExpiresIn = other.Value.ExpiresIn;
				this.ExpiresAt = other.Value.ExpiresAt;
				this.AuthType = other.Value.AuthType;
				this.RefreshToken = other.Value.RefreshToken;
				this.RefreshExpiresIn = other.Value.RefreshExpiresIn;
				this.RefreshExpiresAt = other.Value.RefreshExpiresAt;
			}
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x0003E600 File Offset: 0x0003C800
		public void Dispose()
		{
			Helper.Dispose(ref this.m_App);
			Helper.Dispose(ref this.m_ClientId);
			Helper.Dispose(ref this.m_AccountId);
			Helper.Dispose(ref this.m_AccessToken);
			Helper.Dispose(ref this.m_ExpiresAt);
			Helper.Dispose(ref this.m_RefreshToken);
			Helper.Dispose(ref this.m_RefreshExpiresAt);
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x0003E662 File Offset: 0x0003C862
		public void Get(out Token output)
		{
			output = default(Token);
			output.Set(ref this);
		}

		// Token: 0x0400122A RID: 4650
		private int m_ApiVersion;

		// Token: 0x0400122B RID: 4651
		private IntPtr m_App;

		// Token: 0x0400122C RID: 4652
		private IntPtr m_ClientId;

		// Token: 0x0400122D RID: 4653
		private IntPtr m_AccountId;

		// Token: 0x0400122E RID: 4654
		private IntPtr m_AccessToken;

		// Token: 0x0400122F RID: 4655
		private double m_ExpiresIn;

		// Token: 0x04001230 RID: 4656
		private IntPtr m_ExpiresAt;

		// Token: 0x04001231 RID: 4657
		private AuthTokenType m_AuthType;

		// Token: 0x04001232 RID: 4658
		private IntPtr m_RefreshToken;

		// Token: 0x04001233 RID: 4659
		private double m_RefreshExpiresIn;

		// Token: 0x04001234 RID: 4660
		private IntPtr m_RefreshExpiresAt;
	}
}
