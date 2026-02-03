using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200017E RID: 382
	public struct SessionSearchRemoveParameterOptions
	{
		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x0000FE70 File Offset: 0x0000E070
		// (set) Token: 0x06000B32 RID: 2866 RVA: 0x0000FE78 File Offset: 0x0000E078
		public Utf8String Key { get; set; }

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x0000FE81 File Offset: 0x0000E081
		// (set) Token: 0x06000B34 RID: 2868 RVA: 0x0000FE89 File Offset: 0x0000E089
		public ComparisonOp ComparisonOp { get; set; }
	}
}
