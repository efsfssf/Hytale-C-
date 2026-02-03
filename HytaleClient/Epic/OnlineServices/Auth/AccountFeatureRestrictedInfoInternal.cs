using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200062A RID: 1578
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AccountFeatureRestrictedInfoInternal : IGettable<AccountFeatureRestrictedInfo>, ISettable<AccountFeatureRestrictedInfo>, IDisposable
	{
		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x060028E8 RID: 10472 RVA: 0x0003C094 File Offset: 0x0003A294
		// (set) Token: 0x060028E9 RID: 10473 RVA: 0x0003C0B5 File Offset: 0x0003A2B5
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

		// Token: 0x060028EA RID: 10474 RVA: 0x0003C0C5 File Offset: 0x0003A2C5
		public void Set(ref AccountFeatureRestrictedInfo other)
		{
			this.m_ApiVersion = 1;
			this.VerificationURI = other.VerificationURI;
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x0003C0DC File Offset: 0x0003A2DC
		public void Set(ref AccountFeatureRestrictedInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.VerificationURI = other.Value.VerificationURI;
			}
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x0003C112 File Offset: 0x0003A312
		public void Dispose()
		{
			Helper.Dispose(ref this.m_VerificationURI);
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x0003C121 File Offset: 0x0003A321
		public void Get(out AccountFeatureRestrictedInfo output)
		{
			output = default(AccountFeatureRestrictedInfo);
			output.Set(ref this);
		}

		// Token: 0x0400118B RID: 4491
		private int m_ApiVersion;

		// Token: 0x0400118C RID: 4492
		private IntPtr m_VerificationURI;
	}
}
