using System;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000349 RID: 841
	public struct UpdateModCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001702 RID: 5890 RVA: 0x000218CA File Offset: 0x0001FACA
		// (set) Token: 0x06001703 RID: 5891 RVA: 0x000218D2 File Offset: 0x0001FAD2
		public Result ResultCode { get; set; }

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001704 RID: 5892 RVA: 0x000218DB File Offset: 0x0001FADB
		// (set) Token: 0x06001705 RID: 5893 RVA: 0x000218E3 File Offset: 0x0001FAE3
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06001706 RID: 5894 RVA: 0x000218EC File Offset: 0x0001FAEC
		// (set) Token: 0x06001707 RID: 5895 RVA: 0x000218F4 File Offset: 0x0001FAF4
		public object ClientData { get; set; }

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001708 RID: 5896 RVA: 0x000218FD File Offset: 0x0001FAFD
		// (set) Token: 0x06001709 RID: 5897 RVA: 0x00021905 File Offset: 0x0001FB05
		public ModIdentifier? Mod { get; set; }

		// Token: 0x0600170A RID: 5898 RVA: 0x00021910 File Offset: 0x0001FB10
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x0002192D File Offset: 0x0001FB2D
		internal void Set(ref UpdateModCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.LocalUserId = other.LocalUserId;
			this.ClientData = other.ClientData;
			this.Mod = other.Mod;
		}
	}
}
