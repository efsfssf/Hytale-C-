using System;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000D6 RID: 214
	public struct Stat
	{
		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x0000B1F9 File Offset: 0x000093F9
		// (set) Token: 0x060007BA RID: 1978 RVA: 0x0000B201 File Offset: 0x00009401
		public Utf8String Name { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x0000B20A File Offset: 0x0000940A
		// (set) Token: 0x060007BC RID: 1980 RVA: 0x0000B212 File Offset: 0x00009412
		public DateTimeOffset? StartTime { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x0000B21B File Offset: 0x0000941B
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x0000B223 File Offset: 0x00009423
		public DateTimeOffset? EndTime { get; set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x0000B22C File Offset: 0x0000942C
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x0000B234 File Offset: 0x00009434
		public int Value { get; set; }

		// Token: 0x060007C1 RID: 1985 RVA: 0x0000B23D File Offset: 0x0000943D
		internal void Set(ref StatInternal other)
		{
			this.Name = other.Name;
			this.StartTime = other.StartTime;
			this.EndTime = other.EndTime;
			this.Value = other.Value;
		}
	}
}
