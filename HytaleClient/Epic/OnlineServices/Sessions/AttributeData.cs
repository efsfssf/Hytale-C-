using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000EE RID: 238
	public struct AttributeData
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x0000BB30 File Offset: 0x00009D30
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x0000BB38 File Offset: 0x00009D38
		public Utf8String Key { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x0000BB41 File Offset: 0x00009D41
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x0000BB49 File Offset: 0x00009D49
		public AttributeDataValue Value { get; set; }

		// Token: 0x06000814 RID: 2068 RVA: 0x0000BB52 File Offset: 0x00009D52
		internal void Set(ref AttributeDataInternal other)
		{
			this.Key = other.Key;
			this.Value = other.Value;
		}
	}
}
