using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005CD RID: 1485
	public struct CopyProductUserExternalAccountByAccountTypeOptions
	{
		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x060026A8 RID: 9896 RVA: 0x000393D6 File Offset: 0x000375D6
		// (set) Token: 0x060026A9 RID: 9897 RVA: 0x000393DE File Offset: 0x000375DE
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x060026AA RID: 9898 RVA: 0x000393E7 File Offset: 0x000375E7
		// (set) Token: 0x060026AB RID: 9899 RVA: 0x000393EF File Offset: 0x000375EF
		public ExternalAccountType AccountIdType { get; set; }
	}
}
