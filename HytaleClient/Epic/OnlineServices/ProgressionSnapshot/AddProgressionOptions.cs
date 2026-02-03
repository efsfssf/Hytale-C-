using System;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	// Token: 0x020002A5 RID: 677
	public struct AddProgressionOptions
	{
		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x0001B98D File Offset: 0x00019B8D
		// (set) Token: 0x060012EF RID: 4847 RVA: 0x0001B995 File Offset: 0x00019B95
		public uint SnapshotId { get; set; }

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x060012F0 RID: 4848 RVA: 0x0001B99E File Offset: 0x00019B9E
		// (set) Token: 0x060012F1 RID: 4849 RVA: 0x0001B9A6 File Offset: 0x00019BA6
		public Utf8String Key { get; set; }

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x060012F2 RID: 4850 RVA: 0x0001B9AF File Offset: 0x00019BAF
		// (set) Token: 0x060012F3 RID: 4851 RVA: 0x0001B9B7 File Offset: 0x00019BB7
		public Utf8String Value { get; set; }
	}
}
