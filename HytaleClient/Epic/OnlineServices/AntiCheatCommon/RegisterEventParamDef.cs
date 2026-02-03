using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006C7 RID: 1735
	public struct RegisterEventParamDef
	{
		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06002D18 RID: 11544 RVA: 0x00042A30 File Offset: 0x00040C30
		// (set) Token: 0x06002D19 RID: 11545 RVA: 0x00042A38 File Offset: 0x00040C38
		public Utf8String ParamName { get; set; }

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06002D1A RID: 11546 RVA: 0x00042A41 File Offset: 0x00040C41
		// (set) Token: 0x06002D1B RID: 11547 RVA: 0x00042A49 File Offset: 0x00040C49
		public AntiCheatCommonEventParamType ParamType { get; set; }

		// Token: 0x06002D1C RID: 11548 RVA: 0x00042A52 File Offset: 0x00040C52
		internal void Set(ref RegisterEventParamDefInternal other)
		{
			this.ParamName = other.ParamName;
			this.ParamType = other.ParamType;
		}
	}
}
