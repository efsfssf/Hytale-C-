using System;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x020001A3 RID: 419
	public struct PlayerSanction
	{
		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x000119D1 File Offset: 0x0000FBD1
		// (set) Token: 0x06000C1C RID: 3100 RVA: 0x000119D9 File Offset: 0x0000FBD9
		public long TimePlaced { get; set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000C1D RID: 3101 RVA: 0x000119E2 File Offset: 0x0000FBE2
		// (set) Token: 0x06000C1E RID: 3102 RVA: 0x000119EA File Offset: 0x0000FBEA
		public Utf8String Action { get; set; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x000119F3 File Offset: 0x0000FBF3
		// (set) Token: 0x06000C20 RID: 3104 RVA: 0x000119FB File Offset: 0x0000FBFB
		public long TimeExpires { get; set; }

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x00011A04 File Offset: 0x0000FC04
		// (set) Token: 0x06000C22 RID: 3106 RVA: 0x00011A0C File Offset: 0x0000FC0C
		public Utf8String ReferenceId { get; set; }

		// Token: 0x06000C23 RID: 3107 RVA: 0x00011A15 File Offset: 0x0000FC15
		internal void Set(ref PlayerSanctionInternal other)
		{
			this.TimePlaced = other.TimePlaced;
			this.Action = other.Action;
			this.TimeExpires = other.TimeExpires;
			this.ReferenceId = other.ReferenceId;
		}
	}
}
