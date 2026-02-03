using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200004D RID: 77
	public struct AcknowledgeEventIdOptions
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x000069CB File Offset: 0x00004BCB
		// (set) Token: 0x06000481 RID: 1153 RVA: 0x000069D3 File Offset: 0x00004BD3
		public ulong UiEventId { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x000069DC File Offset: 0x00004BDC
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x000069E4 File Offset: 0x00004BE4
		public Result Result { get; set; }
	}
}
