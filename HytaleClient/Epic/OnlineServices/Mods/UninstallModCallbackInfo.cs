using System;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000345 RID: 837
	public struct UninstallModCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060016E2 RID: 5858 RVA: 0x000215AB File Offset: 0x0001F7AB
		// (set) Token: 0x060016E3 RID: 5859 RVA: 0x000215B3 File Offset: 0x0001F7B3
		public Result ResultCode { get; set; }

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060016E4 RID: 5860 RVA: 0x000215BC File Offset: 0x0001F7BC
		// (set) Token: 0x060016E5 RID: 5861 RVA: 0x000215C4 File Offset: 0x0001F7C4
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060016E6 RID: 5862 RVA: 0x000215CD File Offset: 0x0001F7CD
		// (set) Token: 0x060016E7 RID: 5863 RVA: 0x000215D5 File Offset: 0x0001F7D5
		public object ClientData { get; set; }

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060016E8 RID: 5864 RVA: 0x000215DE File Offset: 0x0001F7DE
		// (set) Token: 0x060016E9 RID: 5865 RVA: 0x000215E6 File Offset: 0x0001F7E6
		public ModIdentifier? Mod { get; set; }

		// Token: 0x060016EA RID: 5866 RVA: 0x000215F0 File Offset: 0x0001F7F0
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x0002160D File Offset: 0x0001F80D
		internal void Set(ref UninstallModCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.LocalUserId = other.LocalUserId;
			this.ClientData = other.ClientData;
			this.Mod = other.Mod;
		}
	}
}
