using System;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x0200032F RID: 815
	public struct EnumerateModsCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x0600164E RID: 5710 RVA: 0x000208C2 File Offset: 0x0001EAC2
		// (set) Token: 0x0600164F RID: 5711 RVA: 0x000208CA File Offset: 0x0001EACA
		public Result ResultCode { get; set; }

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001650 RID: 5712 RVA: 0x000208D3 File Offset: 0x0001EAD3
		// (set) Token: 0x06001651 RID: 5713 RVA: 0x000208DB File Offset: 0x0001EADB
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001652 RID: 5714 RVA: 0x000208E4 File Offset: 0x0001EAE4
		// (set) Token: 0x06001653 RID: 5715 RVA: 0x000208EC File Offset: 0x0001EAEC
		public object ClientData { get; set; }

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001654 RID: 5716 RVA: 0x000208F5 File Offset: 0x0001EAF5
		// (set) Token: 0x06001655 RID: 5717 RVA: 0x000208FD File Offset: 0x0001EAFD
		public ModEnumerationType Type { get; set; }

		// Token: 0x06001656 RID: 5718 RVA: 0x00020908 File Offset: 0x0001EB08
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x00020925 File Offset: 0x0001EB25
		internal void Set(ref EnumerateModsCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.LocalUserId = other.LocalUserId;
			this.ClientData = other.ClientData;
			this.Type = other.Type;
		}
	}
}
