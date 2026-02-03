using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200063B RID: 1595
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct IdTokenInternal : IGettable<IdToken>, ISettable<IdToken>, IDisposable
	{
		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x0600294A RID: 10570 RVA: 0x0003CDA8 File Offset: 0x0003AFA8
		// (set) Token: 0x0600294B RID: 10571 RVA: 0x0003CDC9 File Offset: 0x0003AFC9
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

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x0600294C RID: 10572 RVA: 0x0003CDDC File Offset: 0x0003AFDC
		// (set) Token: 0x0600294D RID: 10573 RVA: 0x0003CDFD File Offset: 0x0003AFFD
		public Utf8String JsonWebToken
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_JsonWebToken, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_JsonWebToken);
			}
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x0003CE0D File Offset: 0x0003B00D
		public void Set(ref IdToken other)
		{
			this.m_ApiVersion = 1;
			this.AccountId = other.AccountId;
			this.JsonWebToken = other.JsonWebToken;
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x0003CE34 File Offset: 0x0003B034
		public void Set(ref IdToken? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AccountId = other.Value.AccountId;
				this.JsonWebToken = other.Value.JsonWebToken;
			}
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x0003CE7F File Offset: 0x0003B07F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AccountId);
			Helper.Dispose(ref this.m_JsonWebToken);
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x0003CE9A File Offset: 0x0003B09A
		public void Get(out IdToken output)
		{
			output = default(IdToken);
			output.Set(ref this);
		}

		// Token: 0x040011C1 RID: 4545
		private int m_ApiVersion;

		// Token: 0x040011C2 RID: 4546
		private IntPtr m_AccountId;

		// Token: 0x040011C3 RID: 4547
		private IntPtr m_JsonWebToken;
	}
}
