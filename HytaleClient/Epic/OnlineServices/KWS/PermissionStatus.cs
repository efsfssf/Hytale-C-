using System;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x0200049D RID: 1181
	public struct PermissionStatus
	{
		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06001EBC RID: 7868 RVA: 0x0002CE87 File Offset: 0x0002B087
		// (set) Token: 0x06001EBD RID: 7869 RVA: 0x0002CE8F File Offset: 0x0002B08F
		public Utf8String Name { get; set; }

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06001EBE RID: 7870 RVA: 0x0002CE98 File Offset: 0x0002B098
		// (set) Token: 0x06001EBF RID: 7871 RVA: 0x0002CEA0 File Offset: 0x0002B0A0
		public KWSPermissionStatus Status { get; set; }

		// Token: 0x06001EC0 RID: 7872 RVA: 0x0002CEA9 File Offset: 0x0002B0A9
		internal void Set(ref PermissionStatusInternal other)
		{
			this.Name = other.Name;
			this.Status = other.Status;
		}
	}
}
