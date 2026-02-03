using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000121 RID: 289
	public enum OnlineSessionState
	{
		// Token: 0x04000448 RID: 1096
		NoSession,
		// Token: 0x04000449 RID: 1097
		Creating,
		// Token: 0x0400044A RID: 1098
		Pending,
		// Token: 0x0400044B RID: 1099
		Starting,
		// Token: 0x0400044C RID: 1100
		InProgress,
		// Token: 0x0400044D RID: 1101
		Ending,
		// Token: 0x0400044E RID: 1102
		Ended,
		// Token: 0x0400044F RID: 1103
		Destroying
	}
}
