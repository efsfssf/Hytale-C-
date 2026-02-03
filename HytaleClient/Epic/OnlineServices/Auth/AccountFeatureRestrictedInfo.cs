using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000629 RID: 1577
	public struct AccountFeatureRestrictedInfo
	{
		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x060028E5 RID: 10469 RVA: 0x0003C071 File Offset: 0x0003A271
		// (set) Token: 0x060028E6 RID: 10470 RVA: 0x0003C079 File Offset: 0x0003A279
		public Utf8String VerificationURI { get; set; }

		// Token: 0x060028E7 RID: 10471 RVA: 0x0003C082 File Offset: 0x0003A282
		internal void Set(ref AccountFeatureRestrictedInfoInternal other)
		{
			this.VerificationURI = other.VerificationURI;
		}
	}
}
