using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005E3 RID: 1507
	public struct GetExternalAccountMappingsOptions
	{
		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06002726 RID: 10022 RVA: 0x00039F4F File Offset: 0x0003814F
		// (set) Token: 0x06002727 RID: 10023 RVA: 0x00039F57 File Offset: 0x00038157
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06002728 RID: 10024 RVA: 0x00039F60 File Offset: 0x00038160
		// (set) Token: 0x06002729 RID: 10025 RVA: 0x00039F68 File Offset: 0x00038168
		public ExternalAccountType AccountIdType { get; set; }

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x0600272A RID: 10026 RVA: 0x00039F71 File Offset: 0x00038171
		// (set) Token: 0x0600272B RID: 10027 RVA: 0x00039F79 File Offset: 0x00038179
		public Utf8String TargetExternalUserId { get; set; }
	}
}
