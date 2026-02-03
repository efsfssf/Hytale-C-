using System;

namespace Epic.OnlineServices.Logging
{
	// Token: 0x0200035B RID: 859
	public struct LogMessage
	{
		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001766 RID: 5990 RVA: 0x00022403 File Offset: 0x00020603
		// (set) Token: 0x06001767 RID: 5991 RVA: 0x0002240B File Offset: 0x0002060B
		public Utf8String Category { get; set; }

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001768 RID: 5992 RVA: 0x00022414 File Offset: 0x00020614
		// (set) Token: 0x06001769 RID: 5993 RVA: 0x0002241C File Offset: 0x0002061C
		public Utf8String Message { get; set; }

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x0600176A RID: 5994 RVA: 0x00022425 File Offset: 0x00020625
		// (set) Token: 0x0600176B RID: 5995 RVA: 0x0002242D File Offset: 0x0002062D
		public LogLevel Level { get; set; }

		// Token: 0x0600176C RID: 5996 RVA: 0x00022436 File Offset: 0x00020636
		internal void Set(ref LogMessageInternal other)
		{
			this.Category = other.Category;
			this.Message = other.Message;
			this.Level = other.Level;
		}
	}
}
