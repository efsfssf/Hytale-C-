using System;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200075E RID: 1886
	public struct StatThresholds
	{
		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06003116 RID: 12566 RVA: 0x00048ED6 File Offset: 0x000470D6
		// (set) Token: 0x06003117 RID: 12567 RVA: 0x00048EDE File Offset: 0x000470DE
		public Utf8String Name { get; set; }

		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06003118 RID: 12568 RVA: 0x00048EE7 File Offset: 0x000470E7
		// (set) Token: 0x06003119 RID: 12569 RVA: 0x00048EEF File Offset: 0x000470EF
		public int Threshold { get; set; }

		// Token: 0x0600311A RID: 12570 RVA: 0x00048EF8 File Offset: 0x000470F8
		internal void Set(ref StatThresholdsInternal other)
		{
			this.Name = other.Name;
			this.Threshold = other.Threshold;
		}
	}
}
