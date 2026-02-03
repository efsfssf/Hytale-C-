using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200014C RID: 332
	public struct SessionDetailsAttribute
	{
		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0000E12D File Offset: 0x0000C32D
		// (set) Token: 0x06000A11 RID: 2577 RVA: 0x0000E135 File Offset: 0x0000C335
		public AttributeData? Data { get; set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x0000E13E File Offset: 0x0000C33E
		// (set) Token: 0x06000A13 RID: 2579 RVA: 0x0000E146 File Offset: 0x0000C346
		public SessionAttributeAdvertisementType AdvertisementType { get; set; }

		// Token: 0x06000A14 RID: 2580 RVA: 0x0000E14F File Offset: 0x0000C34F
		internal void Set(ref SessionDetailsAttributeInternal other)
		{
			this.Data = other.Data;
			this.AdvertisementType = other.AdvertisementType;
		}
	}
}
