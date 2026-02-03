using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006A5 RID: 1701
	public struct LogEventParamPair
	{
		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x06002BBC RID: 11196 RVA: 0x0004064B File Offset: 0x0003E84B
		// (set) Token: 0x06002BBD RID: 11197 RVA: 0x00040653 File Offset: 0x0003E853
		public LogEventParamPairParamValue ParamValue { get; set; }

		// Token: 0x06002BBE RID: 11198 RVA: 0x0004065C File Offset: 0x0003E85C
		internal void Set(ref LogEventParamPairInternal other)
		{
			this.ParamValue = other.ParamValue;
		}
	}
}
