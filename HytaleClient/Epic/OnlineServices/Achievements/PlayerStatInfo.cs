using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000758 RID: 1880
	public struct PlayerStatInfo
	{
		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x060030F0 RID: 12528 RVA: 0x00048B58 File Offset: 0x00046D58
		// (set) Token: 0x060030F1 RID: 12529 RVA: 0x00048B60 File Offset: 0x00046D60
		public Utf8String Name { get; set; }

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x060030F2 RID: 12530 RVA: 0x00048B69 File Offset: 0x00046D69
		// (set) Token: 0x060030F3 RID: 12531 RVA: 0x00048B71 File Offset: 0x00046D71
		public int CurrentValue { get; set; }

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x060030F4 RID: 12532 RVA: 0x00048B7A File Offset: 0x00046D7A
		// (set) Token: 0x060030F5 RID: 12533 RVA: 0x00048B82 File Offset: 0x00046D82
		public int ThresholdValue { get; set; }

		// Token: 0x060030F6 RID: 12534 RVA: 0x00048B8B File Offset: 0x00046D8B
		internal void Set(ref PlayerStatInfoInternal other)
		{
			this.Name = other.Name;
			this.CurrentValue = other.CurrentValue;
			this.ThresholdValue = other.ThresholdValue;
		}
	}
}
