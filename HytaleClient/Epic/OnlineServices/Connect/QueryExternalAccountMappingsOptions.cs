using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000615 RID: 1557
	public struct QueryExternalAccountMappingsOptions
	{
		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06002838 RID: 10296 RVA: 0x0003AF2E File Offset: 0x0003912E
		// (set) Token: 0x06002839 RID: 10297 RVA: 0x0003AF36 File Offset: 0x00039136
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x0600283A RID: 10298 RVA: 0x0003AF3F File Offset: 0x0003913F
		// (set) Token: 0x0600283B RID: 10299 RVA: 0x0003AF47 File Offset: 0x00039147
		public ExternalAccountType AccountIdType { get; set; }

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x0600283C RID: 10300 RVA: 0x0003AF50 File Offset: 0x00039150
		// (set) Token: 0x0600283D RID: 10301 RVA: 0x0003AF58 File Offset: 0x00039158
		public Utf8String[] ExternalAccountIds { get; set; }
	}
}
