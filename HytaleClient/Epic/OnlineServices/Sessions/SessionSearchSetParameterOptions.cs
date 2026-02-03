using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000182 RID: 386
	public struct SessionSearchSetParameterOptions
	{
		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x0000FF95 File Offset: 0x0000E195
		// (set) Token: 0x06000B41 RID: 2881 RVA: 0x0000FF9D File Offset: 0x0000E19D
		public AttributeData? Parameter { get; set; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x0000FFA6 File Offset: 0x0000E1A6
		// (set) Token: 0x06000B43 RID: 2883 RVA: 0x0000FFAE File Offset: 0x0000E1AE
		public ComparisonOp ComparisonOp { get; set; }
	}
}
