using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200053C RID: 1340
	public struct ItemOwnership
	{
		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06002305 RID: 8965 RVA: 0x00033DB5 File Offset: 0x00031FB5
		// (set) Token: 0x06002306 RID: 8966 RVA: 0x00033DBD File Offset: 0x00031FBD
		public Utf8String Id { get; set; }

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06002307 RID: 8967 RVA: 0x00033DC6 File Offset: 0x00031FC6
		// (set) Token: 0x06002308 RID: 8968 RVA: 0x00033DCE File Offset: 0x00031FCE
		public OwnershipStatus OwnershipStatus { get; set; }

		// Token: 0x06002309 RID: 8969 RVA: 0x00033DD7 File Offset: 0x00031FD7
		internal void Set(ref ItemOwnershipInternal other)
		{
			this.Id = other.Id;
			this.OwnershipStatus = other.OwnershipStatus;
		}
	}
}
